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

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;                                             // 装备类型
}
