using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // ���rb��y���ٶ�С��0����״̬���л���player��airState
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
