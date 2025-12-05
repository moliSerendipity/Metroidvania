using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Enemy_Archer enemy;
    protected Transform player;

    public ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;                       // 获取玩家坐标
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // 检测到玩家或距离过小时进入战斗状态
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDistance)
            stateMachine.ChangeState(enemy.battleState);
    }
}
