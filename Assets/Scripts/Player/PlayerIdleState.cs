using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.EndAttack();
        player.SetZeroVelocity();                                          // ����ҵ��ٶ���Ϊ0����ֹ����״̬�˳�ʱ����������
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // ����������ķ�������ҵĳ�����ͬ����Ҽ�⵽ǽ�ڣ��򷵻أ���ֹ���ײǽ
        if (xInput == player.facingDir && player.IsWallDetected())
            return;
        // ����������ķ���Ϊ0����Ҳ�æµ����ֹ����ڹ���ʱ���ƶ�������ı���ҵ�״̬Ϊ�ƶ�״̬
        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
