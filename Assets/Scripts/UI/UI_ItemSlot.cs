using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// UI 装备槽位的逻辑，负责显示物品图标、数量，并响应点击、悬停等事件
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;                                                            // UI 管理器
    [SerializeField] protected Image itemImage;                                 // 槽位里显示的图片（物品图标）
    [SerializeField] protected TextMeshProUGUI itemText;                        // 槽位右下角的文字（物品数量）

    public InventoryItem item;                                                  // 当前槽位绑定的物品（可能为空）

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    /// <summary>
    /// 更新槽位显示内容
    /// </summary>
    /// <param name="_newItem">要绑定的新物品，可以为空</param>
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;                                                        // 绑定到当前格子
        if (item != null)
        {
            // 显示图标
            itemImage.color = Color.white;                                      // 把图片颜色恢复成正常（避免透明状态）
            itemImage.sprite = item.data.icon;                                  // 设置物品图标

            // 显示数量（如果大于 1 才显示数字）
            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";                                             // 单个物品就不显示数量
        }
        else
        {
            // 清空显示
            itemImage.color = Color.clear;                                      // 图标透明
            itemImage.sprite = null;                                            // 清空图标
            itemText.text = "";                                                 // 清空数量
        }
    }

    /// <summary>
    /// 手动清空格子（彻底移除物品）
    /// </summary>
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    /// <summary>
    /// 点击格子时的逻辑
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        // 按住 LeftControl 丢弃物品
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        // 如果是装备：直接装备
        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        // 显示点击后当前格子新物品的提示框，或当格子里无物品时隐藏提示框
        if (item != null)
            ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        else
            ui.itemToolTip.HideToolTip();
    }

    /// <summary>
    /// 鼠标移入格子：显示提示框
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    /// <summary>
    /// 鼠标移出格子：隐藏提示框
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.HideToolTip();
    }
}