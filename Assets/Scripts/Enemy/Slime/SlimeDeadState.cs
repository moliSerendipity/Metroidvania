using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    private Enemy_Slime enemy;

    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // 敌人死亡时保持最后一帧的状态（这是在同一帧退出并进入的动画）
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;                                                       // 定格当前动画
        enemy.cd.enabled = false;                                                   // 取消碰撞体
        stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        // 如果stateTimer>0，就向上飞
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}
