using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ö�����пɱ� Buff ����������
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

// Buff Ч��������ĳ������һ��ʱ�䣩
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item Effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    private Dictionary<StatType, Stat> statMap;                                     // ���ֵ�ӳ�� StatType �� Stat
    [SerializeField] private StatType buffType;                                     // �������������
    [SerializeField] private float buffAmount;                                      // ������ֵ
    [SerializeField] private float buffDuration;                                    // Buff ����ʱ��

    // ִ�� Buff Ч��
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // ȷ���ֵ��ѳ�ʼ��
        if (statMap == null) InitStatMap();

        // ���� buffType �ҵ���Ӧ����
        if (statMap.TryGetValue(buffType, out Stat targetStat))
            stats.IncreaseStatBy(targetStat, buffAmount, buffDuration);
        else
            Debug.LogWarning($"δ�ҵ� StatType {buffType} ��Ӧ������ӳ�䣡");
    }

    // ��ʼ���ֵ�ӳ��
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
