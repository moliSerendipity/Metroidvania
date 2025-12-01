using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;                                            // 攻击时的小移动
    public float counterAttackDuration = 0.2f;                                  // 反击持续时间
    public bool IsAttacking { get; private set; }                               // 是否正在攻击

    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float swordReturnImpact;                                             // 抓住剑时的冲击力大小
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;                                                  // 冲刺持续时间
    public float dashDir { get; private set; }                                  // 冲刺方向
    private float defaultDashSpeed;

    public SkillManager skill { get; private set; }                             // 技能管理器
    public GameObject sword { get; private set; }                               // 游戏物体，剑

    #region States
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerSlideWallState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackhole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        // 初始化状态机和状态
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerSlideWallState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackhole = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }
    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;                                          // 获取技能管理器
        stateMachine.Initialize(idleState);                                     // 初始化状态机，进入初始状态

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    
    protected override void Update()
    {
        if (Time.timeScale == 0) return;
        base.Update();

        stateMachine.currentState.Update();                                     // 更新当前状态
        CheckForDashInput();                                                    // 检测冲刺输入

        if (Input.GetKeyDown(KeyCode.U) && skill.crystal.crystalUnlcoked)
            skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))                                   // 按 1 使用药瓶
            Inventory.instance.UseFlask();
    }

    // 给该脚本的GameObject类型的sword变量赋值
    public void AssignNewSword(GameObject _newSword)
    {
        if (sword == null)
            sword = _newSword;
    }

    // 抓剑并销毁剑
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);                                   // 切换为抓剑的状态
        Destroy(sword);                                                         // 销毁该脚本的GameObject类型的sword
    }

    // 忙碌状态
    public IEnumerator BusyFor(float _seconds)                                  
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // 动画结束触发的事件
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // 开始攻击
    public void StartAttack()
    {
        IsAttacking = true;
    }

    // 攻击结束
    public void EndAttack()
    {
        IsAttacking = false;
    }

    // 检测冲刺输入            
    private void CheckForDashInput()                                            
    {
        if (IsWallDetected() || stateMachine.currentState == blackhole)         // 如果检测到墙壁或在使用黑洞，则返回，禁止冲刺
            return;
        if (skill.dash.dashUnlocked == false)                                   // 如果 dash 技能未解锁，则返回
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() )
        {
            dashDir = Input.GetAxisRaw("Horizontal");                           // 冲刺方向
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);                                // 切换到冲刺状态
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    // 死亡，进入死亡状态
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
