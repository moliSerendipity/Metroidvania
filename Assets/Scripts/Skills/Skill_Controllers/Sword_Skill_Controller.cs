using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;                                              // �Ƿ�����ת
    private bool isReturning;                                                   // �Ƿ����ڻ���
    private float returnSpeed = 12;                                             // ���չ����н����˶��ٶ�
    private float maxTravelDistance;                                            // ��Զ�ɴ����
    private float checkLifeTimer;                                               // ����Ƿ񳬳���Զ�����ʱ��
    private float checkLifeInterval = 0.2f;                                     // ����Ƿ񳬳���Զ����ļ����
    private float freezeTimeDuration;                                           // �������ʱ��������

    [Header("Bounce info")]
    private float bounceSpeed;                                                  // ����ʱ���ƶ��ٶ�
    private bool isBouncing;                                                    // �Ƿ��ڵ���
    private int bounceAmount;                                                   // �ɵ������
    private List<Transform> enemyTarget;                                        // ������������
    private int targetIndex;                                                    // Ŀ������

    [Header("Pierce info")]
    private int pierceAmount;                                                   // �ɴ�������

    [Header("Spin info")]
    private float maxSpinDistance;                                              // ��ת���ɴ����Զ����
    private float spinDuration;                                                 // ��ת����ʱ��
    private float spinTimer;                                                    // ��ת��ʱ��
    private bool isStopped;                                                     // �Ƿ�ֹͣ�ƶ�
    private bool isSpinning;                                                    // �Ƿ�������ת
    private float hitTimer;                                                     // ��ת������ʱ��
    private float hitCooldown;                                                  // ��ת�������
    private float spinDirection;                                                // ��ת�����ƶ�����

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // ����Ƿ񳬳���Զ����
        checkLifeTimer -= Time.deltaTime;
        if (checkLifeTimer <= 0)
        {
            checkLifeTimer = checkLifeInterval;
            if ((transform.position - player.transform.position).sqrMagnitude > maxTravelDistance * maxTravelDistance)
            {
                Destroy(gameObject);
                return;
            }
        }

        // �������ת��������ġ��ҷ��򡱶�׼��ǰ���ٶȷ���
        if (canRotate)
            transform.right = rb.velocity;

        // ��������ڻ��գ����ý����Ž�ɫ�����ƶ�
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            // �����ɫ�뽣�ľ����Сʱ�����л�Ϊץ��״̬������Player�ű���GameObject���͵�sword�����������ٴ�������Ľ�
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();                                                          // ���佣�������߼�
        SpinLogic();                                                            // ��ת���������߼�
    }

    // �����ܵ��������˺�
    private void SwordSkillDamage(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.DamageEffect();                                                     // �������������ܵ��˺�
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);          // ������˵�ʱ���������
    }

    // ���佣�������߼�
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            // ������佣�����е��ˣ��ͻ��ڵ���֮����е��䣬ÿ�����е���Ŀ������+1���ɵ������-1��
            // ����ɵ������<=0��isBouncing����Ϊfalse��isReturning����Ϊtrue�����ս�
            // �����������Ŀ�����ޣ��ʹӵ�һ�����˼�������
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    // ��ת���������߼�
    private void SpinLogic()
    {
        if (isSpinning)
        {
            // �����ת��������Զ���뻹δ�򵽵��ˣ���ͣ��
            if (Vector2.Distance(transform.position, player.transform.position) > maxSpinDistance && !isStopped)
                SetupForSpinningWhenCollided();

            if (isStopped)
            {
                // �����ת��ͣ�£��ͳ����ӳ��ķ����ƶ�
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                // ��ת��ʱ������ʱ�����<0�����ͷ���
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }

                // ��ת������ʱ������ʱ�����<0�����¿�ʼ��ʱ�����������ĵ������һ���˺�
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (Collider2D hit in colliders)
                    {
                        SwordSkillDamage(hit.gameObject.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    // ��û��ֹͣ�ƶ������������˻����ʱ������ͣ����
    private void SetupForSpinningWhenCollided()
    {
        isStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    // ���н�������
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed,
        float _maxTravelDistance)
    {
        rb.velocity = _dir;                                                     // ���ķ���
        rb.gravityScale = _gravityScale;                                        // ��������
        player = _player;                                                       // ��ҽű�
        freezeTimeDuration = _freezeTimeDuration;                               // �������ʱ��������
        returnSpeed = _returnSpeed;                                             // ���չ����н����˶��ٶ�
        maxTravelDistance = _maxTravelDistance;                                 // ��Զ�ɴ����
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);                                     // ������Ǵ��̽������Ž�����ת����
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);                      // ��ת�������ӳ��ķ����ƶ�

        Destroy(gameObject, 7);
    }

    // ���佣�Ķ�������
    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }

    // ���̽��Ķ�������
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    // ��ת���Ķ�������
    public void SetupSpin(bool _isSpinning, float _maxSpinDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxSpinDistance = _maxSpinDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    // �����ӳ�ȥ�Ľ�
    public void ReturnSword()
    {   // ����xyz�ᣬ����Vector2.MoveTowards���գ���ֹ���յ�����������׹�ղ�����
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;                                                // �øý�����ײ����������������Ϊ����������
        isReturning = true;                                                     // �Ƿ����ڻ�������Ϊtrue
        isSpinning = false;                                                     // ��ֹ������ת��ʱ�������ĵ�������˺�
    }

    // ����ǵ��佣���Ͱѵ�һ�������ĵ��˸��������е��ˣ������Լ�����������ӵ�enemyTarget�б�
    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D hit in colliders)
                {
                    if (hit.GetComponent<Enemy>())
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    // ������������ʱ������
    private void StuckInto(Collider2D collision)
    {
        // ������Ŀɴ�������>0���ɴ�������-1�������أ����ı佣���κ����������
        if (pierceAmount >0 && collision.GetComponent<Enemy>())
        {
            pierceAmount--;
            return;
        }
        // �������ת��������ת�����ҽ�û��ֹͣ�ƶ������������˻����ʱ������ͣ���������أ����ı佣���κ����������
        if (isSpinning)
        {
            if (!isStopped &&(collision.GetComponent<Enemy>() || collision.gameObject.layer == 3))
                SetupForSpinningWhenCollided();
            return;
        }

        canRotate = false;                                                      // �Ƿ�����ת����Ϊfalse
        cd.enabled = false;                                                     // ������ײ�����
        rb.isKinematic = true;                                                  // rb��body type����ΪKinematic
        rb.constraints = RigidbodyConstraints2D.FreezeAll;                      // ����xyz��

        // ������ڵ��䣬�ͼ������Ž�����ת���������øý���Ϊ��ײ�����������
        if (isBouncing && enemyTarget.Count >0)
            return;
        anim.SetBool("Rotation", false);                                        // ��ֹ���Ž�����ת����
        transform.parent = collision.transform;                                 // �øý���Ϊ��ײ�����������
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)                                                        // ������ڻ��ս�����ֱ�ӷ���
            return;

        Enemy enemy;
        if(enemy = collision.GetComponent<Enemy>())
            SwordSkillDamage(enemy);

        // ����ǵ��佣���Ͱѵ�һ�������ĵ��˸��������е��ˣ������Լ�����������ӵ�enemyTarget�б�
        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }
}
