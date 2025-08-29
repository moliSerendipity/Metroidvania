using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ч�������ٷֱȻָ�������
[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Heal")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;                                     // �ָ���ռ��������İٷֱ�

    // ִ������Ч��
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // ���������� = �������ֵ * �ٷֱ�
        float healAmount = playerStats.maxHealth.GetValue() * healPercent;
        playerStats.IncreaseHealthBy(healAmount);                                   // �ظ�����
    }
}
