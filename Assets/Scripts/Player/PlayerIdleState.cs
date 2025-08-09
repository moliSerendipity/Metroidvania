using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.EndAttack();
        player.SetZeroVelocity();                                          // 将玩家的速度置为0，防止其他状态退出时非正常滑行
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // 如果玩家输入的方向与玩家的朝向相同且玩家检测到墙壁，则返回，禁止玩家撞墙
        if (xInput == player.facingDir && player.IsWallDetected())
            return;
        // 如果玩家输入的方向不为0且玩家不忙碌（防止玩家在攻击时能移动），则改变玩家的状态为移动状态
        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
