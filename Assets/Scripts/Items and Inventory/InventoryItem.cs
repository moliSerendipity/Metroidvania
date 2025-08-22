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

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();                                                                 // ��ʼ��ʱĬ������ +1
    }

    public void AddStack() => stackSize++;                                          // ���� +1
    public void RemoveStack() => stackSize--;                                       // ���� -1
}
