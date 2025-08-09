using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;                                                    // �Ƿ���Դ�����¡��

    public PlayerCounterAttackState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;                                                      // ���Դ�����¡��
        stateTimer = player.counterAttackDuration;                                  // ������ҷ�������ʱ��
        player.anim.SetBool("SuccessfulCounterAttack", false);                      // ���óɹ�������������Ϊfalse
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();                                                   // ������ʱ��ֹ�ƶ�
        // ��ȡ������Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        // ����������ײ��
        foreach (Collider2D hit in colliders)
        {
            // �����ײ���ǵ������ܱ�����
            if (hit.GetComponent<Enemy>())
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())                        // ����ܱ����ξͽ���ѣ��״̬
                {
                    stateTimer = 10f;                                               // ��һ����ֵ�����ⷴ���ɹ�����û����
                    player.anim.SetBool("SuccessfulCounterAttack", true);           // ���óɹ�������������Ϊtrue
                    if (canCreateClone)
                    {
                        // �Ƿ��ڷ���ʱ������¡��
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                        canCreateClone = false;                                     // �����ٴ�����¡��,ֻ�ܴ���һ��
                    }
                }
            }
        }

        // ���û�����ɹ���stateTimer�ĳ�ֵΪcounterAttackDuration���򷴻��ɹ���stateTimerΪ10�����ͽ��뾲ֹ״̬
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
