using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;                              // 玩家图层

    [Header("Stunned info")]
    public float stunDuration;                                                      // 晕眩持续时间
    public Vector2 stunDirection;                                                   // 晕眩方向
    protected bool canBeStunned;                                                    // 是否可以被晕眩
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;                                                         // 移动速度
    public float idleTime;                                                          // 静止时间
    public float battleTime;                                                        // 进入战斗状态持续时间
    private float defaultMoveSpeed;                                                 // 初始移动速度

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;                                                    // 攻击CD
    [HideInInspector] public float lastTimeAttacked;                                // 上次攻击结束时间

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }                            // 上次动画布尔参数名

    protected override void Awake()
    {
        base.Awake();

        defaultMoveSpeed = moveSpeed;
        // 初始化状态机
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();                                         // 更新当前状态
    }

    // 是否冻结敌人的时间，让其移动速度和动画播放速度都为0
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    // 冻结敌人的时间持续几秒
    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    // 打开反击/攻击窗口
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;                                                        // 可以被击晕
        counterImage.SetActive(true);                                               // 反击/攻击提示图片可见
    }

    // 关闭反击/攻击窗口
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;                                                       // 不可被击晕
        counterImage.SetActive(false);                                              // 隐藏反击/攻击提示图片
    }
    #endregion

    // 是否可以被击晕
    public virtual bool CanBeStunned()
    {
        // 如果可以被击晕，则关闭反击/攻击窗口，并返回true
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // 动画结束触发的事件
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // 记录当前动画参数名
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    // 检测玩家
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50f, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;                                                // 将攻击范围射线改为黄色
        // 绘制攻击范围射线
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + facingDir * attackDistance, transform.position.y));
    }
}
