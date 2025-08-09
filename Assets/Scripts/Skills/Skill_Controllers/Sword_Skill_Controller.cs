using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;                                              // 是否能旋转
    private bool isReturning;                                                   // 是否正在回收
    private float returnSpeed = 12;                                             // 回收过程中剑的运动速度
    private float maxTravelDistance;                                            // 最远可达距离
    private float checkLifeTimer;                                               // 检测是否超出最远距离计时器
    private float checkLifeInterval = 0.2f;                                     // 检测是否超出最远距离的检测间隔
    private float freezeTimeDuration;                                           // 冻结敌人时间持续多久

    [Header("Bounce info")]
    private float bounceSpeed;                                                  // 弹射时的移动速度
    private bool isBouncing;                                                    // 是否在弹射
    private int bounceAmount;                                                   // 可弹射次数
    private List<Transform> enemyTarget;                                        // 敌人坐标数组
    private int targetIndex;                                                    // 目标索引

    [Header("Pierce info")]
    private int pierceAmount;                                                   // 可穿刺数量

    [Header("Spin info")]
    private float maxSpinDistance;                                              // 旋转剑可达的最远距离
    private float spinDuration;                                                 // 旋转持续时间
    private float spinTimer;                                                    // 旋转计时器
    private bool isStopped;                                                     // 是否停止移动
    private bool isSpinning;                                                    // 是否正在旋转
    private float hitTimer;                                                     // 旋转攻击计时器
    private float hitCooldown;                                                  // 旋转攻击间隔
    private float spinDirection;                                                // 旋转剑的移动方向

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // 检测是否超出最远距离
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

        // 如果能旋转，让物体的“右方向”对准当前的速度方向
        if (canRotate)
            transform.right = rb.velocity;

        // 如果剑正在回收，就让剑朝着角色方向移动
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            // 如果角色与剑的距离较小时，则切换为抓剑状态并销毁Player脚本的GameObject类型的sword变量，即销毁创造出来的剑
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();                                                          // 弹射剑的运行逻辑
        SpinLogic();                                                            // 旋转剑的运行逻辑
    }

    // 敌人受到剑技的伤害
    private void SwordSkillDamage(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.DamageEffect();                                                     // 敌人碰到剑会受到伤害
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);          // 冻结敌人的时间持续几秒
    }

    // 弹射剑的运行逻辑
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            // 如果弹射剑附近有敌人，就会在敌人之间进行弹射，每次命中敌人目标索引+1，可弹射次数-1，
            // 如果可弹射次数<=0，isBouncing设置为false，isReturning设置为true，回收剑
            // 如果索引超过目标上限，就从第一个敌人继续弹射
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

    // 旋转剑的运行逻辑
    private void SpinLogic()
    {
        if (isSpinning)
        {
            // 如果旋转剑到达最远距离还未打到敌人，就停下
            if (Vector2.Distance(transform.position, player.transform.position) > maxSpinDistance && !isStopped)
                SetupForSpinningWhenCollided();

            if (isStopped)
            {
                // 如果旋转剑停下，就朝着扔出的方向移动
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                // 旋转计时器倒计时，如果<0，剑就返回
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }

                // 旋转攻击计时器倒计时，如果<0，重新开始计时，并对碰到的敌人造成一次伤害
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

    // 剑没有停止移动，当碰到敌人或地面时就让它停下来
    private void SetupForSpinningWhenCollided()
    {
        isStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    // 进行剑的设置
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed,
        float _maxTravelDistance)
    {
        rb.velocity = _dir;                                                     // 剑的方向
        rb.gravityScale = _gravityScale;                                        // 剑的重力
        player = _player;                                                       // 玩家脚本
        freezeTimeDuration = _freezeTimeDuration;                               // 冻结敌人时间持续多久
        returnSpeed = _returnSpeed;                                             // 回收过程中剑的运动速度
        maxTravelDistance = _maxTravelDistance;                                 // 最远可达距离
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);                                     // 如果不是穿刺剑，播放剑的旋转动画
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);                      // 旋转剑朝着扔出的方向移动

        Destroy(gameObject, 7);
    }

    // 弹射剑的额外设置
    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }

    // 穿刺剑的额外设置
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    // 旋转剑的额外设置
    public void SetupSpin(bool _isSpinning, float _maxSpinDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxSpinDistance = _maxSpinDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    // 回收扔出去的剑
    public void ReturnSword()
    {   // 冻结xyz轴，仅靠Vector2.MoveTowards回收，防止剑收到重力持续下坠收不回来
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;                                                // 让该剑从碰撞到的物体的子物体成为独立的物体
        isReturning = true;                                                     // 是否正在回收设置为true
        isSpinning = false;                                                     // 防止回收旋转剑时对碰到的敌人造成伤害
    }

    // 如果是弹射剑，就把第一个碰到的敌人附近的所有敌人（包括自己）的坐标添加到enemyTarget列表
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

    // 当剑碰到物体时的设置
    private void StuckInto(Collider2D collision)
    {
        // 如果剑的可穿刺数量>0，可穿刺数量-1，并返回，不改变剑的任何组件和属性
        if (pierceAmount >0 && collision.GetComponent<Enemy>())
        {
            pierceAmount--;
            return;
        }
        // 如果是旋转剑且在旋转，并且剑没有停止移动，当碰到敌人或地面时就让它停下来并返回，不改变剑的任何组件和属性
        if (isSpinning)
        {
            if (!isStopped &&(collision.GetComponent<Enemy>() || collision.gameObject.layer == 3))
                SetupForSpinningWhenCollided();
            return;
        }

        canRotate = false;                                                      // 是否能旋转设置为false
        cd.enabled = false;                                                     // 禁用碰撞器组件
        rb.isKinematic = true;                                                  // rb的body type设置为Kinematic
        rb.constraints = RigidbodyConstraints2D.FreezeAll;                      // 冻结xyz轴

        // 如果剑在弹射，就继续播放剑的旋转动画，不让该剑成为碰撞物体的子物体
        if (isBouncing && enemyTarget.Count >0)
            return;
        anim.SetBool("Rotation", false);                                        // 禁止播放剑的旋转动画
        transform.parent = collision.transform;                                 // 让该剑成为碰撞物体的子物体
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)                                                        // 如果正在回收剑，就直接返回
            return;

        Enemy enemy;
        if(enemy = collision.GetComponent<Enemy>())
            SwordSkillDamage(enemy);

        // 如果是弹射剑，就把第一个碰到的敌人附近的所有敌人（包括自己）的坐标添加到enemyTarget列表
        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }
}
