using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // ��������ʱ�������һ֡��״̬��������ͬһ֡�˳�������Ķ�����
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;                                                       // ����ǰ����
        enemy.cd.enabled = false;                                                   // ȡ����ײ��
        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        // ���stateTimer>0�������Ϸ�
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}
