using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
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
        
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);               // ������ҵ��ٶ�
        // ���xInputΪ0������Ҽ�⵽ǽ�ڣ���ı�״̬ΪidleState
        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
