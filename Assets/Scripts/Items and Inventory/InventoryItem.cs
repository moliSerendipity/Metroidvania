using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����д洢����Ʒʵ��
[Serializable]
public class InventoryItem
{
    public ItemData data;                                                           // ��Ʒ��������
    public int stackSize;                                                           // ��������

    public InventoryItem(ItemData _newItemData, int amount = 1)
    {
        data = _newItemData;
        stackSize = amount;                                                         // ��ʼ��ʱָ������
    }

    public void AddStack(int amount = 1) => stackSize += amount;                    // ���� +amount
    public void RemoveStack(int amount = 1) => stackSize -= amount;                 // ���� -amount
}
