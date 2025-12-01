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
            // 如果按下空格键，改变状态为wallJump，并返回，防止不正确的状态切换
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        // 如果x轴输入不为0且玩家朝向与x轴输入方向不同，改变状态为idleState
        if (xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState);

        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);                        // 如果向下按，则加速下降
        else
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);                 // 否则缓慢下降

        // 如果检测到地面，改变状态为idleState
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        // 如果未检测到墙壁且未触地，改变状态为airState下降
        if (!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);
    }
}
