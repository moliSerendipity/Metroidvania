using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // ����ʱ��������������룬��ɸı�����ĳ���ͺ����ٶ�
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);

        // ����ʱ�������Ҽ�⵽ǽ�ڣ���ı�״̬Ϊ���ǽ�ڻ���״̬
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        
        // ����ʱ�������Ҽ�⵽���棬��ı�״̬Ϊ�������״̬
        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

    }
}
