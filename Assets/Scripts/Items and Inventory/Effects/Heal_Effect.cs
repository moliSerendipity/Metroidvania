using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 治疗效果（按百分比恢复生命）
[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Heal")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;                                     // 恢复量占最大生命的百分比

    // 执行治疗效果
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // 计算治疗量 = 最大生命值 * 百分比
        float healAmount = playerStats.maxHealth.GetValue() * healPercent;
        playerStats.IncreaseHealthBy(healAmount);                                   // 回复生命
    }
}
