using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 装备类型
public enum EquipmentType
{
    Weapon,                                                                         // 武器
    Armor,                                                                          // 防具
    Amulet,                                                                         // 护身符
    Flask                                                                           // 药水瓶
}

// 装备数据（继承自 ItemData）
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;                                             // 装备类型
    public ItemEffect[] itemEffects;                                                // 装备附带的效果

    [Header("Major stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Offensive stats")]
    public float damage;                                                            // 攻击力
    public float critChance;                                                        // 暴击率
    public float critDamage;                                                        // 暴击伤害

    [Header("Defensive stats")]
    public float health;                                                            // 最大生命值
    public float armor;                                                             // 护甲值
    public float evasion;                                                           // 闪避率
    public float magicResistance;

    [Header("Magic stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;                                   // 合成所需材料

    // 执行装备绑定的所有效果
    public void Effect(Transform _enemyPosition)
    {
        foreach (ItemEffect item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    // 穿戴装备时，把装备属性加到玩家属性上
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critDamage.AddModifier(critDamage);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    // 卸下装备时，把装备属性从玩家属性中移除
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critDamage.RemoveModifier(critDamage);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }
}
