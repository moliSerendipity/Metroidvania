using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image itemImage;                                 // 槽位里显示的图片（物品图标）
    [SerializeField] private TextMeshProUGUI itemText;                          // 槽位右下角的文字（物品数量）

    public InventoryItem item;                                                  // 当前槽位绑定的物品（可能为空）

    // 更新槽位的显示内容
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
            itemImage.color = Color.clear;                                      // 把图片隐藏
            itemImage.sprite = null;                                            // 清空图标
            itemText.text = "";                                                 // 清空数量
        }
    }

    // 手动清空格子
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    // 点击格子时触发逻辑
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // 按住 LeftControl 丢弃物品
        if (item != null && Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        // 装备物品
        if (item != null && item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }
}
