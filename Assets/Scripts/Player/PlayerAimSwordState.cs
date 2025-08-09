using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);                                        // 使剑的运动轨迹点可见
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.2f));                                // 玩家在扔出剑后0.2s内无法移动
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();                                                   // 静止玩家在瞄准的时候移动

        // 如果松开右键，就回到静止状态
        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        // 瞄准时面对鼠标朝向
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if (mousePosition.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();
    }
}
