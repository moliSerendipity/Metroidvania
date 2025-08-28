using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;                                                // 水晶存在时间计时器
    private bool canExplode;                                                        // 是否可以爆炸
    private bool canMoveToEnemy;                                                    // 是否可以朝敌人位置移动
    private float moveSpeed;                                                        // 移动速度

    private bool canGrow;                                                           // 技能结束时是否可以生长
    private float growSpeed = 5f;                                                   // 生长速度

    private Transform closestTarget;                                                // 最近的敌人
    [SerializeField] private LayerMask whatIsEnemy;                                 // 敌人层

    // 设置水晶参数
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
        // 技能持续时间到就销毁自身
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        // 如果可以移动，就朝最近敌人位置移动，如果距离过小，则停止移动并销毁
        if (canMoveToEnemy && closestTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1.5)
            {
                canMoveToEnemy = false;
                FinishCrystal();
            }
        }

        // 如果爆炸，就放大自身（爆炸动画）
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    // 选择随机敌人
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    // 技能结束时如果可以爆炸，则允许生长并播放爆炸动画，否则直接销毁
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

    // 销毁水晶自身
    public void SelfDestroy() => Destroy(gameObject);

    // 当水晶爆炸时，检测范围内的敌人，造成伤害，产生装备效果
    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius * transform.localScale.x);
        foreach(Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>())
            {
                player.stats.DoMagicDamage(hit.GetComponent<EntityStats>());
                // 产生 Amulet 的效果
                Inventory.instance.GetEquipment(EquipmentType.Amulet)?.Effect(hit.transform);
            }
        }
    }
}
