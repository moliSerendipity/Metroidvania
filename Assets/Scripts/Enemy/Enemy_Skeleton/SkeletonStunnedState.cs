using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : 
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);                                             // ��˸
        stateTimer = enemy.stunDuration;                                                                // ѣ��ʱ��
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);     // ѣ��ʱλ��
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedBlink", 0);                                       // ȡ����˸�����ԭ������ɫ
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)                                                         // ѣ�ν�����ص���ֹ״̬
            stateMachine.ChangeState(enemy.idleState);
    }
}
