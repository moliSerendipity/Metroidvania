using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLossSpeed;                                  // ��¡��Ӱ��ɫ˥���ٶ�
    private float cloneTimer;                                                       // ��¡��Ӱ��ʱ��

    [SerializeField] private Transform attackCheck;                                 // ��������
    [SerializeField] private float attackCheckRadius = 0.8f;                        // �������뾶
    private Transform closestEnemy;                                                 // �������
    private int facingDir = 1;                                                      // ����

    private bool canDuplicateClone;                                                 // �Ƿ���Կ�¡��Ӱ
    private float chanceToDuplicate;                                                // ��¡��Ӱ����

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        // �����¡��Ӱ���ʱ���������������ʧ
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - colorLossSpeed * Time.deltaTime);
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }

    }

    // �����¼�
    private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "AnimationTrigger":
                AnimationTrigger();
                break;
            case "AttackTrigger":
                AttackTrigger();
                break;
            default:
                break;
        }
    }

    private void AnimationTrigger()
    {
        cloneTimer = -1;
    }

    // �������������¼�
    private void AttackTrigger()
    {
        // ��ȡ������Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        // ����������ײ��
        foreach (Collider2D hit in colliders)
        {
            // �����ײ���Ϲ���Enemy�ű�������Enemy�ű���Damage������������Կ�¡��Ӱ������һ�����ʿ�¡��Ӱ
            if (hit.GetComponent<Enemy>())
            {
                hit.GetComponent<Enemy>().DamageEffect();

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                }
            }
        }
    }

    // ���п�¡�������
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1,4));                     // ���ѡ�񹥻�����

        transform.position = _newTransform.position + _offset;                      // ��¡��Ӱλ��
        cloneTimer = _cloneDuration;                                                // ���ÿ�¡��Ӱ��ʱ��
        closestEnemy = _closestEnemy;                                               // �����������
        canDuplicateClone = _canDuplicateClone;                                     // �����Ƿ���Կ�¡��Ӱ
        chanceToDuplicate = _chanceToDuplicate;                                     // ���ÿ�¡��Ӱ����

        FaceClosestTarget();                                                        // ��������ĵ���
    }

    // �����������
    private void FaceClosestTarget()
    {
        // ���������˴��ڣ���������
        if(closestEnemy != null)
        {
            // ������ʼ�����ұߣ����ɫ�ƶ������޹أ���ֻ��������ĵ����ұߣ���Ҫ��ת
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
