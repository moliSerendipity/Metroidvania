using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    private string animBoolName;                                            // ��������ֵ����

    protected float stateTimer;                                             // ״̬��ʱ��
    protected bool triggerCalled;                                           // �������Ƿ񱻵���
    protected Rigidbody2D rb;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;                                                  // ��ֹ������������
        rb = enemyBase.rb;                                                      // ��ȡ���˸���
        enemyBase.anim.SetBool(animBoolName, true);                             // ���ö�������ֵΪ�棬���Ŷ���
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);                            // ���ö�������ֵΪ�٣�ֹͣ����
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;                                                   // ������������
    }
}
