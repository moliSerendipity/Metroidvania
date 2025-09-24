using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    // UI 组件绑定：物品名称、物品类型、物品描述
    [SerializeField] private TextMeshProUGUI itemNameText;                          // 显示物品名称
    [SerializeField] private TextMeshProUGUI itemTypeText;                          // 显示物品类型（比如武器、防具）
    [SerializeField] private TextMeshProUGUI itemDescription;                       // 显示物品描述

    void Start()
    {
        
    }

    /// <summary>
    /// 显示物品提示框
    /// </summary>
    /// <param name="item">要展示的物品（装备类数据）</param>
    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null) return;

        // 更新 UI 内容
        itemNameText.text = item.itemName;                                          // 设置名称
        itemTypeText.text = item.equipmentType.ToString();                          // 设置类型（枚举转字符串）
        itemDescription.text = item.GetDescription();                               // 设置描述（调用物品的数据方法）

        gameObject.SetActive(true);                                                 // 激活提示框，让其显示在画面上
    }

    /// <summary>
    /// 隐藏物品提示框
    /// </summary>
    public void HideToolTip() => gameObject.SetActive(false);                       // 直接禁用 GameObject
}
