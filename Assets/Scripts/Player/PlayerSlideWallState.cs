using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideWallState : PlayerState
{
    public PlayerSlideWallState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            // ������¿ո�����ı�״̬ΪwallJump�������أ���ֹ����ȷ��״̬�л�
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        // ���x�����벻Ϊ0����ҳ�����x�����뷽��ͬ���ı�״̬ΪidleState
        if (xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState);

        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);                        // ������°���������½�
        else
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);                 // �������½�

        // �����⵽���棬�ı�״̬ΪidleState
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        // ���δ��⵽ǽ����δ���أ��ı�״̬ΪairState�½����̳�û�ӣ�
        if (!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);
    }
}
