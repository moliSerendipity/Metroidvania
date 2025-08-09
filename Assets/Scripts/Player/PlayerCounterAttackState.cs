using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;                                                    // 是否可以创建克隆体

    public PlayerCounterAttackState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;                                                      // 可以创建克隆体
        stateTimer = player.counterAttackDuration;                                  // 设置玩家反击持续时间
        player.anim.SetBool("SuccessfulCounterAttack", false);                      // 设置成功反击动画参数为false
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();                                                   // 反击的时候静止移动
        // 获取攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        // 遍历所有碰撞体
        foreach (Collider2D hit in colliders)
        {
            // 如果碰撞体是敌人且能被击晕
            if (hit.GetComponent<Enemy>())
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())                        // 如果能被击晕就进入眩晕状态
                {
                    stateTimer = 10f;                                               // 给一个大值，避免反击成功动画没播完
                    player.anim.SetBool("SuccessfulCounterAttack", true);           // 设置成功反击动画参数为true
                    if (canCreateClone)
                    {
                        // 是否在反击时创建克隆体
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                        canCreateClone = false;                                     // 不能再创建克隆体,只能创建一次
                    }
                }
            }
        }

        // 如果没反击成功（stateTimer的初值为counterAttackDuration）或反击成功（stateTimer为10），就进入静止状态
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
