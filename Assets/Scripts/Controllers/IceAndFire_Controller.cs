using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : MonoBehaviour
{
    // ������⵽����ʱ���Ե������һ��ħ���˺�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicDamage(enemyTarget);
        }
    }
}
