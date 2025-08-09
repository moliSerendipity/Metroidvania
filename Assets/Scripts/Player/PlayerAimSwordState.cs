using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);                                        // ʹ�����˶��켣��ɼ�
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.2f));                                // ������ӳ�����0.2s���޷��ƶ�
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();                                                   // ��ֹ�������׼��ʱ���ƶ�

        // ����ɿ��Ҽ����ͻص���ֹ״̬
        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        // ��׼ʱ�����곯��
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();
        else if (mousePosition.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();
    }
}
