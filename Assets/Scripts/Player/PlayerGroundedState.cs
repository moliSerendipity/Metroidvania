using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // ������R��ʱ����״̬���л���player.blackhole״̬
        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackhole);

        // ����������Ҽ���δ������ʱ����״̬���л���player.aimSword״̬��HasNoSword()���Ի��ս�
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSword);

        // ������Q��ʱ����״̬���л���player.counterAttack״̬
        if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttack);

        // ������������ʱ����״̬���л���player.primaryAttack״̬
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.IsAttacking)
        {
            stateMachine.ChangeState(player.primaryAttack);
            player.StartAttack();                                                   // ����IsAttackingΪtrue
            return;
        }

        // ����Ҳ��ڵ�����ʱ����״̬���л���player.airState״̬
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        // �����¿ո��������ڵ�����ʱ����״̬���л���player.jumpState״̬
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    // �Ƿ�û�ж������������������Ҫ���ս�
    private bool HasNoSword()
    {
        if (!player.sword)
            return true;
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
