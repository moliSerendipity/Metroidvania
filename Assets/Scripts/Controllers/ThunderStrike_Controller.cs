using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 雷击特效控制器（负责雷击触发的逻辑）
public class ThunderStrike_Controller : MonoBehaviour
{
    // 触发检测到敌人时，对敌人造成一次魔法伤害
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
