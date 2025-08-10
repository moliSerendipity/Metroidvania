using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : 
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;                   // ��ȡ�������
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;                                  // �������ڼ�ⷶΧ�ڣ�������ս��ʱ��
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())                                            // �����ҽ��빥����Χ�����Թ���������ͽ��빥��״̬
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            // ���ս��ʱ������������󣬹���ͽ��뾲ֹ״̬
            if (stateTimer < 0 || Vector2.Distance(player.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState);
        }

        // ������ҷ����ƶ�
        if (player.position.x > enemy.transform.position.x + 0.1f)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x - 0.1f)
            moveDir = -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    // �ж��Ƿ���Թ���
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            // �������CD�������͸��¹���ʱ�䣬������true����ʾ���Թ���
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
