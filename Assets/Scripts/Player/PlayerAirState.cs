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

        // 下落时，如果横向有输入，则可改变下落的朝向和横向速度
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);

        // 下落时，如果玩家检测到墙壁，则改变状态为玩家墙壁滑动状态
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        
        // 下落时，如果玩家检测到地面，则改变状态为玩家闲置状态
        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

    }
}
