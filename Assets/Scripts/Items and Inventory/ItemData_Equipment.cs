using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// װ������
public enum EquipmentType
{
    Weapon,                                                                         // ����
    Armor,                                                                          // ����
    Amulet,                                                                         // �����
    Flask                                                                           // ҩˮƿ
}

// װ�����ݣ��̳��� ItemData��
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;                                             // װ������
    public ItemEffect[] itemEffects;                                                // װ��������Ч��

    [Header("Major stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Offensive stats")]
    public float damage;                                                            // ������
    public float critChance;                                                        // ������
    public float critDamage;                                                        // �����˺�

    [Header("Defensive stats")]
    public float health;                                                            // �������ֵ
    public float armor;                                                             // ����ֵ
    public float evasion;                                                           // ������
    public float magicResistance;

    [Header("Magic stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;                                   // �ϳ��������

    // ִ��װ���󶨵�����Ч��
    public void Effect(Transform _enemyPosition)
    {
        foreach (ItemEffect item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    // ����װ��ʱ����װ�����Լӵ����������
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

    // ж��װ��ʱ����װ�����Դ�����������Ƴ�
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
