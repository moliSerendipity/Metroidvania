using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    private string animBoolName;                                            // 动画布尔值名称

    protected float stateTimer;                                             // 状态计时器
    protected bool triggerCalled;                                           // 触发器是否被调用
    protected Rigidbody2D rb;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;                                                  // 禁止触发器被调用
        rb = enemyBase.rb;                                                      // 获取敌人刚体
        enemyBase.anim.SetBool(animBoolName, true);                             // 设置动画布尔值为真，播放动画
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);                            // 设置动画布尔值为假，停止动画
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;                                                   // 触发器被调用
    }
}
