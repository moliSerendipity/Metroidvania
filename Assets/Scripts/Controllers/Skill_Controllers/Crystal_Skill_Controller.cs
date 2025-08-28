using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;                                                // ˮ������ʱ���ʱ��
    private bool canExplode;                                                        // �Ƿ���Ա�ը
    private bool canMoveToEnemy;                                                    // �Ƿ���Գ�����λ���ƶ�
    private float moveSpeed;                                                        // �ƶ��ٶ�

    private bool canGrow;                                                           // ���ܽ���ʱ�Ƿ��������
    private float growSpeed = 5f;                                                   // �����ٶ�

    private Transform closestTarget;                                                // ����ĵ���
    [SerializeField] private LayerMask whatIsEnemy;                                 // ���˲�

    // ����ˮ������
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    private void Update()
    {
        // ���ܳ���ʱ�䵽����������
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        // ��������ƶ����ͳ��������λ���ƶ�����������С����ֹͣ�ƶ�������
        if (canMoveToEnemy && closestTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1.5)
            {
                canMoveToEnemy = false;
                FinishCrystal();
            }
        }

        // �����ը���ͷŴ�������ը������
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    // ѡ���������
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    // ���ܽ���ʱ������Ա�ը�����������������ű�ը����������ֱ������
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    // ����ˮ������
    public void SelfDestroy() => Destroy(gameObject);

    // ��ˮ����ըʱ����ⷶΧ�ڵĵ��ˣ�����˺�������װ��Ч��
    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius * transform.localScale.x);
        foreach(Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>())
            {
                player.stats.DoMagicDamage(hit.GetComponent<EntityStats>());
                // ���� Amulet ��Ч��
                Inventory.instance.GetEquipment(EquipmentType.Amulet)?.Effect(hit.transform);
            }
        }
    }
}
