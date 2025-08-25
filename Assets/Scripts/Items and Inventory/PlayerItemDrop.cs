using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;                               // �������

    // ���ɵ���
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        // ��ǰ��װ���б�����������ֹ�޸�ԭʼװ���б�
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList().ToList();

        // ������ǰװ���б�
        for (int i = 0; i < currentEquipment.Count; i++)
        {
            InventoryItem item = currentEquipment[i];
            if (Random.Range(0, 100) < chanceToLoseItems)                           // ��������ж�
            {
                DropItem(item.data);                                                // ���ɵ�������
                inventory.UnequipItem(item.data as ItemData_Equipment);             // ��ʧװ��
            }
        }

        inventory.UpdateEquipmentSlotUI();                                          // ����װ���� UI
    }
}
