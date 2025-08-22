using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;                                   // 槽位里显示的图片（物品图标）
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
}
