using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������ƷЧ�����ɼ̳���չ��
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    // ִ����ƷЧ������������д�����߼���
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {

    }
}
