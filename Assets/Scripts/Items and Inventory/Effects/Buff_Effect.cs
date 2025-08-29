using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用来枚举所有可被 Buff 的属性类型
public enum StatType
{
    strength,
    agility,
    intellience,
    vitality,

    damage,
    critChance,
    critDamage,

    health,
    armor,
    evasion,
    magicResistance,

    fireDamage,
    iceDamage,
    lightningDamage
}

// Buff 效果（增加某个属性一段时间）
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item Effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    private Dictionary<StatType, Stat> statMap;                                     // 用字典映射 StatType → Stat
    [SerializeField] private StatType buffType;                                     // 增益的属性类型
    [SerializeField] private float buffAmount;                                      // 增益数值
    [SerializeField] private float buffDuration;                                    // Buff 持续时间

    // 执行 Buff 效果
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // 确保字典已初始化
        if (statMap == null) InitStatMap();

        // 根据 buffType 找到对应属性
        if (statMap.TryGetValue(buffType, out Stat targetStat))
            stats.IncreaseStatBy(targetStat, buffAmount, buffDuration);
        else
            Debug.LogWarning($"未找到 StatType {buffType} 对应的属性映射！");
    }

    // 初始化字典映射
    private void InitStatMap()
    {
        statMap = new Dictionary<StatType, Stat>()
        {
            { StatType.strength, stats.strength },
            { StatType.agility, stats.agility },
            { StatType.intellience, stats.intelligence },
            { StatType.vitality, stats.vitality },

            { StatType.damage, stats.damage },
            { StatType.critChance, stats.critChance },
            { StatType.critDamage, stats.critDamage },

            { StatType.health, stats.maxHealth },
            { StatType.armor, stats.armor },
            { StatType.evasion, stats.evasion },
            { StatType.magicResistance, stats.magicResistance },

            { StatType.fireDamage, stats.fireDamage },
            { StatType.iceDamage, stats.iceDamage },
            { StatType.lightningDamage, stats.lightningDamage },
        };
    }
}
