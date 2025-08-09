using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyStateMachine
{
    public EnemyState currentState;                                         // �洢��ǰ״̬

    // ��ʼ���������������ó�ʼ״̬
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    // �ı�״̬�ķ��������������µ�״̬
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
