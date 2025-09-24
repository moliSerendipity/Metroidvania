using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// 装备槽位（继承自 UI_ItemSlot），用于显示装备，并且处理点击卸下的逻辑
public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;                                                  // 槽位类型（武器/护甲/护符/药瓶）

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    // 点击事件（重写 UI_ItemSlot 的点击逻辑）
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null || itemImage.sprite == null)          // 没有装备就直接返回
            return;

        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);            // 把当前装备卸下
        Inventory.instance.AddItem(item.data as ItemData_Equipment);                // 把卸下的装备重新放回背包
        ui.itemToolTip.HideToolTip();                                               // 隐藏物品提示框
        CleanUpSlot();                                                              // 清空槽位
    }
}
