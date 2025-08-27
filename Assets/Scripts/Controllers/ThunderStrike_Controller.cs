using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �׻���Ч�������������׻��������߼���
public class ThunderStrike_Controller : MonoBehaviour
{
    // ������⵽����ʱ���Ե������һ��ħ���˺�
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicDamage(enemyTarget);
        }
    }
}
