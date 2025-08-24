using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 背包中存储的物品实例
[Serializable]
public class InventoryItem
{
    public ItemData data;                                                           // 物品数据引用
    public int stackSize;                                                           // 叠加数量

    public InventoryItem(ItemData _newItemData, int amount = 1)
    {
        data = _newItemData;
        stackSize = amount;                                                         // 初始化时指定数量
    }

    public void AddStack(int amount = 1) => stackSize += amount;                    // 数量 +amount
    public void RemoveStack(int amount = 1) => stackSize -= amount;                 // 数量 -amount
}
