using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;                                                    // ��������

    public PlayerCatchSwordState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;
        // ����ʱ��Խ���λ��
        if (sword.position.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if (sword.position.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();

        // �����һ��ץס���ĳ�����ĸо�
        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.1f));                            // �����ץס����0.1s���޷��ƶ�
    }

    public override void Update()
    {
        base.Update();

        // ������������꣬�ͻص���ֹ״̬
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
