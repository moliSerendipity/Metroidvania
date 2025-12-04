using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected Enemy_Slime enemy;

    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;                                     // 记录攻击结束时间
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();                                                // 停止移动
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
