using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    private Enemy_Shady enemy;
    private int moveDir;

    private float defaultSpeed;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        defaultSpeed = enemy.moveSpeed;                                     // 记录默认移动速度
        enemy.moveSpeed = enemy.battleStateMoveSpeed;
        player = PlayerManager.instance.player.transform;                   // 获取玩家坐标
        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = defaultSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;                                  // 如果玩家在检测范围内，就重置战斗时间
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                enemy.stats.KillEntity();
                return;
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

        // 朝着玩家方向移动
        if (player.position.x > enemy.transform.position.x + 0.1f)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x - 0.1f)
            moveDir = -1;
        
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
