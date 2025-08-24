using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 物品类型（大类）
public enum ItemType
{
    Material,                                                                       // 材料类物品
    Equipment                                                                       // 装备类物品
}

// 通用物品数据基类，使用 ScriptableObject 来定义数据模板
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;                                                       // 物品类型
    public string itemName;                                                         // 物品名称
    public Sprite icon;                                                             // 物品图标

    [Range(0, 100)]
    public float dropChance;                                                        // 掉落概率
}
