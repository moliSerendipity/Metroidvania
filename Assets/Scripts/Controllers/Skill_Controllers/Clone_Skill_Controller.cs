using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLossSpeed;                                  // 克隆残影颜色衰减速度
    private float cloneTimer;                                                       // 克隆残影计时器
    private float attackMultiplier;

    [SerializeField] private Transform attackCheck;                                 // 攻击检测点
    [SerializeField] private float attackCheckRadius = 0.8f;                        // 攻击检测半径
    private Transform closestEnemy;                                                 // 最近敌人
    private int facingDir = 1;                                                      // 方向

    private bool canDuplicateClone;                                                 // 是否可以克隆残影
    private float chanceToDuplicate;                                                // 克隆残影概率

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        // 如果克隆残影存活时间结束，则慢慢消失
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - colorLossSpeed * Time.deltaTime);
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }

    }

    // 动画事件
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

    // 攻击触发动画事件
    private void AttackTrigger()
    {
        // 获取攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        // 遍历所有碰撞体
        foreach (Collider2D hit in colliders)
        {
            // 如果碰撞体上挂有Enemy脚本，让Enemy受伤，如果可以克隆残影，则有一定概率克隆残影
            if (hit.GetComponent<Enemy>())
            {
                //player.stats.DoDamage(hit.GetComponent<EntityStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);
                if (player.skill.clone.canApplyOnHitEffect)
                    Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                }
            }
        }
    }

    // 进行克隆体的设置
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, 
        bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1,4));                     // 随机选择攻击动画

        transform.position = _newTransform.position + _offset;                      // 克隆残影位置
        cloneTimer = _cloneDuration;                                                // 重置克隆残影计时器
        closestEnemy = _closestEnemy;                                               // 设置最近敌人
        canDuplicateClone = _canDuplicateClone;                                     // 设置是否可以克隆残影
        chanceToDuplicate = _chanceToDuplicate;                                     // 设置克隆残影概率
        player = _player;
        attackMultiplier = _attackMultiplier;

        FaceClosestTarget();                                                        // 面向最近的敌人
    }

    // 面向最近敌人
    private void FaceClosestTarget()
    {
        // 如果最近敌人存在，则面向它
        if(closestEnemy != null)
        {
            // 动画初始面向右边（与角色移动朝向无关），只有在最近的敌人右边，才要翻转
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
