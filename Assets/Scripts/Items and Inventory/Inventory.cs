using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 背包管理类（单例）
public class Inventory : MonoBehaviour
{
    public static Inventory instance;                                               // 单例，方便全局调用
    public List<InventoryItem> inventoryItems;                                      // 背包物品列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;                 // 快速查找（Key = ItemData）

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;                         // UI槽位的父物体

    private UI_ItemSlot[] itemSlot;                                                 // 槽位数组（对应 UI）

    private void Awake()
    {
        // 单例模式，确保只有一个 Inventory
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();                                 // 初始化物品列表
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();            // 初始化字典

        // 获取 UI 槽位（父物体下的所有槽位脚本）
        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    // 更新 UI 槽位显示
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
            itemSlot[i].UpdateSlot(inventoryItems[i]);                              // 把物品显示到对应的 UI 槽位
    }

    // 添加物品
    public void AddItem(ItemData _item)
    {
        // 如果物品已经存在字典中，则数量 +1
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // 不存在，则创建新物品实例，加入背包
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }

    public void RemoveItem(ItemData _item)
    {
        // 如果背包里有这个物品
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                // 如果只剩最后一个，就从列表和字典中删除
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();                                                // 否则只减少数量
        }

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }
}
