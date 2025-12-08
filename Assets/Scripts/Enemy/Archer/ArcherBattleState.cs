using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Enemy_Archer enemy;
    private Transform player;
    private int moveDir;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;                   // 获取玩家坐标
        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;                                  // 如果玩家在检测范围内，就重置战斗时间

            if (enemy.IsPlayerDetected().distance < enemy.safeJumpDistance)   // 如果玩家距离过近，尝试跳跃躲避
            {
                if (CanJump())
                {
                    stateMachine.ChangeState(enemy.jumpState);
                    return;
                }
            }

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())                                            // 如果玩家进入攻击范围并可以攻击，怪物就进入攻击状态
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else
        {
            // 如果战斗时间结束或距离过大，怪物就进入静止状态
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 15 || enemy.IsWallDetected())
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        BattleStateFlipControll();
    }

    private void BattleStateFlipControll()
    {
        //朝着玩家方向
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
    }

    // 判断是否可以攻击
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);  // 随机生成下次攻击的CD时间
            // 如果攻击CD结束，就更新攻击时间，并返回true，表示可以攻击
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    private bool CanJump()
    {
        if (enemy.GroundBehindCheck() == false || enemy.WallBehindCheck() == true) return false;

        if (Time.time >= enemy.lastJumpTime + enemy.jumpCooldown)
        {
            enemy.lastJumpTime = Time.time;
            return true;
        }
        return false;
    }
}
