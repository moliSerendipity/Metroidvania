using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = 0.4f;                                                   // ����״̬����ʱ��
    private bool skillUsed;                                                         // �����Ƿ���ʹ��
    private float defaultGravity;                                                   // Ĭ������ֵ

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

        // ����״̬��ʱ�������ü���ʹ�ñ�־������Ĭ��������ȡ������Ч��
        stateTimer = flyTime;
        skillUsed = false;
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        // �ָ�Ĭ��������ȡ�����͸��Ч��
        rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        // �������״̬δ�����������Ϸ�
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);
        else
        {
            // ����Ӧ��΢С�������ٶ�
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                // ���������δʹ�ò�����ʹ�ã���ʹ�úڶ�����
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        // ����ڶ���������ɣ��л�������״̬
        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
