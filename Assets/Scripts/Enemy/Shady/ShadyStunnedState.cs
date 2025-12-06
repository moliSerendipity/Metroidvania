using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    private Enemy_Shady enemy;

    public ShadyStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);                                             // 闪烁
        stateTimer = enemy.stunDuration;                                                                // 眩晕时间
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);     // 眩晕时位移
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelColorChange", 0);                                    // 取消闪烁并变回原来的颜色
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)                                                         // 眩晕结束后回到静止状态
            stateMachine.ChangeState(enemy.idleState);
    }
}
