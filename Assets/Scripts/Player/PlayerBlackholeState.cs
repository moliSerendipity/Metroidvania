using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;                                                   // 飞行状态持续时间
    private bool skillUsed;                                                         // 技能是否已使用
    private float defaultGravity;                                                   // 默认重力值

    public PlayerBlackholeState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        // 设置状态计时器，重置技能使用标志，保存默认重力并取消重力效果
        stateTimer = flyTime;
        skillUsed = false;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        // 恢复默认重力，取消玩家透明效果
        rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        // 如果飞行状态未结束，则向上飞
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        else
        {
            // 否则应用微小的向下速度
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                // 如果技能尚未使用并可以使用，则使用黑洞技能
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        // 如果黑洞技能已完成，切换到空中状态
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
