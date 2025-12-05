using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMoveState : ArcherGroundedState
{
    public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);      // 设置移动速度
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            // 如果前方没路，就转向并切换成静止状态
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
