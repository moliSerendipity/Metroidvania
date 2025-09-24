using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 制作系统中的槽位（继承自 UI_ItemSlot），用于显示要制作的装备，并处理点击制作的逻辑
public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 设置合成槽的显示内容
    /// </summary>
    /// <param name="_data">要展示的装备数据</param>
    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }

    /// <summary>
    /// 当点击合成槽时触发逻辑
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        // 点击时，把该装备的数据传递给合成窗口，更新 UI
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
