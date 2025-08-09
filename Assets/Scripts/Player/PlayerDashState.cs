using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateCloneOnDashStart();                                // ��̿�ʼʱ�Ƿ����²�Ӱ
        stateTimer = player.dashDuration;                                           // ������ҳ�̵ĳ���ʱ��
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.clone.CreateCloneOnDashOver();                                 // ��̽���ʱ�Ƿ����²�Ӱ
        player.SetVelocity(0, rb.velocity.y);                                       // ������ҵĺ����ٶ�Ϊ0����ֹ��̽������������
    }

    public override void Update()
    {
        base.Update();

        // ������û�м�⵽���棬���Ҽ�⵽ǽ�ڣ���ı�״̬ΪwallSlide
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);                   // ������ҵ��ٶ�
        // �����̽�������ı�״̬ΪidleState
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
