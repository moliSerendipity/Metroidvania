using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 制作系统中的槽位（继承自 UI_ItemSlot），用于显示要制作的装备，并处理点击制作的逻辑
public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);                                                           // 当 UI 启用时，刷新一下显示
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;             // 当前槽位绑定的装备
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);        // 调用背包的制造逻辑
    }
}
