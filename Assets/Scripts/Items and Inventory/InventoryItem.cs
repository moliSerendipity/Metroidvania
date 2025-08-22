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

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();                                                                 // 初始化时默认数量 +1
    }

    public void AddStack() => stackSize++;                                          // 数量 +1
    public void RemoveStack() => stackSize--;                                       // 数量 -1
}
