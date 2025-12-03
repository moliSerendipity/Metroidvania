using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;                                                    // 剑的坐标

    public PlayerCatchSwordState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.fx.PlayDustFX();
        player.fx.ScreenShake(player.fx.shakeSwordImpact);
        sword = player.sword.transform;
        // 回收时面对剑的位置
        if (sword.position.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if (sword.position.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();

        // 制造出一种抓住剑的冲击力的感觉
        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.1f));                            // 玩家在抓住剑的0.1s内无法移动
    }

    public override void Update()
    {
        base.Update();

        // 如果动画播放完，就回到静止状态
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
