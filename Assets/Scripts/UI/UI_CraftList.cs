using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;                             // 用来存放所有 CraftSlot 的父物体（容器）
    [SerializeField] private GameObject craftSlotPrefab;                            // 合成槽预制体
    [SerializeField] private List<ItemData_Equipment> craftEquipment;               // 可合成的装备列表

    void Start()
    {   // 自己在父物体中的索引是 0（第一个）
        if (transform.GetSiblingIndex() == 0) 
        {
            SetupCraftList();                                                       // 刷新合成列表 UI
            SetupDefaultCraftWindow();                                              // 默认显示第一个物品的合成详情
        }
    }

    /// <summary>
    /// 刷新合成列表 UI，把 craftEquipment 里的物品生成到 CraftSlot 里
    /// </summary>
    public void SetupCraftList()
    {
        // 清空旧的槽位（避免重复生成）
        for (int i = 0; i < craftSlotParent.childCount; i++)
            Destroy(craftSlotParent.GetChild(i).gameObject);

        // 遍历装备列表，每个装备生成一个 CraftSlot
        for (int i = 0;i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);     // 生成新槽位
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]); // 设置显示内容
        }
    }

    /// <summary>
    /// 默认显示第一个物品的合成详情
    /// </summary>
    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }

    /// <summary>
    /// 当点击 CraftList 时触发
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();                                                           // 刷新合成列表
        SetupDefaultCraftWindow();                                                  // 显示默认合成详情
    }
}