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

        player.skill.clone.CreateCloneOnDashStart();                                // 冲刺开始时是否留下残影
        stateTimer = player.dashDuration;                                           // 设置玩家冲刺的持续时间
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.clone.CreateCloneOnDashOver();                                 // 冲刺结束时是否留下残影
        player.SetVelocity(0, rb.velocity.y);                                       // 设置玩家的横向速度为0，防止冲刺结束后继续滑行
    }

    public override void Update()
    {
        base.Update();

        // 如果玩家没有检测到地面，并且检测到墙壁，则改变状态为wallSlide
        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);                   // 设置玩家的速度
        // 如果冲刺结束，则改变状态为idleState
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
