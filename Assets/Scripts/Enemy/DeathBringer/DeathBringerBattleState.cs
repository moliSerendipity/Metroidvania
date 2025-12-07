using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;
    private int moveDir;

    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;                   // 获取玩家坐标
        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.SetBool("Idle", false);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;                                  // 如果玩家在检测范围内，就重置战斗时间
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())                                            // 如果玩家进入攻击范围并可以攻击，怪物就进入攻击状态
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }

        // 朝着玩家方向移动
        if (player.position.x > enemy.transform.position.x + 0.1f)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x - 0.1f)
            moveDir = -1;
        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - 0.1f)
        {
            enemy.anim.SetBool("Move", false);
            enemy.anim.SetBool("Idle", true);
            return;
        }
        enemy.anim.SetBool("Move", true);
        enemy.anim.SetBool("Idle", false);
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
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
}
