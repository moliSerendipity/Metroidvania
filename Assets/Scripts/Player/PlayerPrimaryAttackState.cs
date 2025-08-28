using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerGroundedState
{
    public int comboCounter { get; private set; }                               // ������
    private float lastTimeAttacked;                                             // �ϴι�������ʱ��
    private float comboWindow = 2f;                                             // �������ʱ����
    public PlayerPrimaryAttackState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // �����������������2�����������������������ʱ�䣬����������������Ϊ0
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);                   // ���ö���������������
  
        float attackDir = player.facingDir;                                     // ��ȡ��������
        if (Input.GetAxisRaw("Horizontal") != 0)                                // ��������룬��ʹ������ķ���
            attackDir = xInput;

        // ʹ��ɫ�ڹ���ʱ��ǰ�ƶ�
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;                                                      // ��ɫ�ڹ���ʱ��ǰ�ƶ��ܳ�����ʱ��
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.15f));                           // ��ɫ����æµ״̬������0.15�룬�ڹ��������в������ɿ����ƶ�
        comboCounter++;                                                         // ������������1
        lastTimeAttacked = Time.time;                                           // ��¼�ϴι���ʱ��
    }

    public override void Update()
    {
        base.Update();

        // �������ʱ��ǰ�ƶ���ʱ�䳬��stateTimer�����ֹ��ɫ�ƶ�
        if (stateMachine.currentState == this && stateTimer < 0)
            player.SetZeroVelocity();

        // ���������״̬�ı䣬���������������꣬���л�������״̬
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
