using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 冰冻敌人效果（条件触发，冻结范围内敌人）
[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item Effect/Freeze enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;                                        // 冰冻持续时间

    // 执行冰冻效果
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // 限制条件：玩家血量低于 10% 才能触发，并且护甲效果当前可用
        if (playerStats.currentHealth > playerStats.maxHealth.GetValue() * 0.1f || !Inventory.instance.CanUseArmor())
            return;

        // 检测范围内所有敌人（半径 2 的圆形范围）
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        // 让所有检测到的敌人被冻结一段时间
        foreach (Collider2D hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
