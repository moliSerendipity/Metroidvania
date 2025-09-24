using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 合成窗口 UI 的显示和逻辑绑定
public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;                              // 合成结果的名字
    [SerializeField] private TextMeshProUGUI itemDescription;                       // 合成结果的描述
    [SerializeField] private Image itemIcon;                                        // 合成结果的图标
    [SerializeField] private Image[] materialImage;                                 // 材料槽位（图片 + 数量）
    [SerializeField] private Button craftButton;                                    // 合成按钮

    /// <summary>
    /// 设置合成窗口的显示内容
    /// </summary>
    /// <param name="_data">要合成的装备数据</param>
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();                                   // 清空按钮上之前绑定的点击事件，避免重复绑定

        // 先把所有材料槽清空（隐藏）
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;                                   // 隐藏图片
            // 隐藏文字
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        // 遍历当前物品的合成材料，填充 UI
        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            // 设置材料图标
            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            // 设置材料数量文字
            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        // 更新结果物品的信息
        itemIcon.sprite = _data.icon;                                               // 显示结果物品的图标
        itemName.text = _data.itemName;                                             // 显示名字
        itemDescription.text = _data.GetDescription();                              // 显示描述

        // 给合成按钮绑定逻辑：点击时检查是否能合成
        // （调用 Inventory.instance.CanCraft 来判断是否材料足够并执行合成）
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
