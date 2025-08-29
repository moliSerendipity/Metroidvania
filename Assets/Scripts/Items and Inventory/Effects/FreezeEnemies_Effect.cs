using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ��������Ч�����������������᷶Χ�ڵ��ˣ�
[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item Effect/Freeze enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;                                        // ��������ʱ��

    // ִ�б���Ч��
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // �������������Ѫ������ 10% ���ܴ��������һ���Ч����ǰ����
        if (playerStats.currentHealth > playerStats.maxHealth.GetValue() * 0.1f || !Inventory.instance.CanUseArmor())
            return;

        // ��ⷶΧ�����е��ˣ��뾶 2 ��Բ�η�Χ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        // �����м�⵽�ĵ��˱�����һ��ʱ��
        foreach (Collider2D hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
