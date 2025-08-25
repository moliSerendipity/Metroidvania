using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;                               // 掉落概率

    // 生成掉落
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        // 当前的装备列表（拷贝），防止修改原始装备列表
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList().ToList();

        // 遍历当前装备列表
        for (int i = 0; i < currentEquipment.Count; i++)
        {
            InventoryItem item = currentEquipment[i];
            if (Random.Range(0, 100) < chanceToLoseItems)                           // 掉落概率判断
            {
                DropItem(item.data);                                                // 生成掉落物体
                inventory.UnequipItem(item.data as ItemData_Equipment);             // 丢失装备
            }
        }

        inventory.UpdateEquipmentSlotUI();                                          // 更新装备槽 UI
    }
}
