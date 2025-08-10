using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;                                            // ����ʱ��С�ƶ�
    public float counterAttackDuration = 0.2f;                                  // ��������ʱ��
    public bool IsAttacking { get; private set; }                               // �Ƿ����ڹ���

    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float swordReturnImpact;                                             // ץס��ʱ�ĳ������С

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;                                                  // ��̳���ʱ��
    public float dashDir { get; private set; }                                  // ��̷���

    public SkillManager skill { get; private set; }                             // ���ܹ�����
    public GameObject sword { get; private set; }                               // ��Ϸ���壬��

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

        // ��ʼ��״̬����״̬
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

        skill = SkillManager.instance;                                          // ��ȡ���ܹ�����
        stateMachine.Initialize(idleState);                                     // ��ʼ��״̬���������ʼ״̬
    }

    
    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();                                     // ���µ�ǰ״̬
        CheckForDashInput();                                                    // ���������

        if (Input.GetKeyDown(KeyCode.U))
            skill.crystal.CanUseSkill();
    }

    // ���ýű���GameObject���͵�sword������ֵ
    public void AssignNewSword(GameObject _newSword)
    {
        if (sword == null)
            sword = _newSword;
    }

    // ץ�������ٽ�
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);                                   // �л�Ϊץ����״̬
        Destroy(sword);                                                         // ���ٸýű���GameObject���͵�sword
    }

    // æµ״̬
    public IEnumerator BusyFor(float _seconds)                                  
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // ���������������¼�
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // ��ʼ����
    public void StartAttack()
    {
        IsAttacking = true;
    }

    // ��������
    public void EndAttack()
    {
        IsAttacking = false;
    }

    // ���������            
    private void CheckForDashInput()                                            
    {
        if (IsWallDetected() || stateMachine.currentState == blackhole)         // �����⵽ǽ�ڻ���ʹ�úڶ����򷵻أ���ֹ���
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() )
        {
            dashDir = Input.GetAxisRaw("Horizontal");                           // ��̷���
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);                                // �л������״̬
        }
    }

    // ��������������״̬
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
