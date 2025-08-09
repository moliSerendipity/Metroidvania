using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected float xInput;                                             // ˮƽ����
    protected float yInput;                                             // ��ֱ����
    protected Rigidbody2D rb;
    private string animBoolName;                                        // ��������ֵ����
    protected float stateTimer;                                         // ״̬��ʱ��
    protected bool triggerCalled;                                       // �������Ƿ񱻵���

    public PlayerState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName)
    {
        player = __player;
        stateMachine = __stateMachine;
        animBoolName = _animBoolName;
    }

    // ����״̬ʱ�Ĳ���
    public virtual void Enter()
    {
        rb = player.rb;                                                 // ��ȡ��Ҹ���
        player.anim.SetBool(animBoolName, true);                        // ���ö�������ֵΪ�棬���Ŷ���
        triggerCalled = false;                                          // ��ֹ������������
    }

    // ״̬����ʱ�Ĳ���
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");    
        player.anim.SetFloat("yVelocity", rb.velocity.y);               // ���ö�����ֱ�ٶ�
    }

    // �˳�״̬ʱ�Ĳ���
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);                       // ���ö�������ֵΪ�٣�ֹͣ����
    }

    // ���������������¼�
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;                                           // ������������
    }
}
