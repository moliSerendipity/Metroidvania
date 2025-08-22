using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 背包管理类（单例）
public class Inventory : MonoBehaviour
{
    public static Inventory instance;                                               // 单例，方便全局调用

    public List<InventoryItem> equipment;                                           // 已装备物品列表
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;       // 装备字典（按类型快速查找）

    public List<InventoryItem> inventory;                                           // 背包物品列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;                 // 快速查找（Key = ItemData）

    public List<InventoryItem> stash;                                               // 仓库物品列表
    public Dictionary <ItemData, InventoryItem> stashDictionary;                    // 仓库字典

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;                         // 背包 UI 槽位父物体
    [SerializeField] private Transform stashSlotParent;                             // 仓库 UI 槽位父物体
    [SerializeField] private Transform equipmentSlotParent;                         // 装备 UI 槽位父物体

    private UI_ItemSlot[] inventoryItemSlot;                                        // 背包槽位
    private UI_ItemSlot[] stashItemSlot;                                            // 仓库槽位
    private UI_EquipmentSlot[] equipmentSlot;                                       // 装备槽位

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
        // 初始化三类列表和字典
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        // 获取 UI 槽位（父物体下的所有槽位脚本）
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    // 装备物品逻辑
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;              // 强制转型（非装备会返回 null）
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment OldEquipment = null;

        // 找到同类型的旧装备（武器/护甲等）
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                OldEquipment = item.Key;
        }

        // 如果有旧装备，卸下并放回背包
        if (OldEquipment != null)
        {
            if (equipmentDictionary.TryGetValue(OldEquipment, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(OldEquipment);
            }

            AddItem(OldEquipment);                                                  // 卸下的装备放回背包
        }

        // 添加新装备
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);

        RemoveItem(_item);                                                          // 从背包移除刚装备的物品
        //UpdateSlotUI();
    }

    // 更新 UI 槽位显示
    private void UpdateSlotUI()
    {
        // 先清空所有槽位
        for (int i = 0; i < inventoryItemSlot.Length; i++)
            inventoryItemSlot[i].CleanUpSlot();
        for (int i = 0; i < stashItemSlot.Length; i++)
            stashItemSlot[i].CleanUpSlot();

        // 逐一填充背包 & 仓库槽位
        for (int i = 0; i < inventory.Count; i++)
            inventoryItemSlot[i].UpdateSlot(inventory[i]);                          // 把物品显示到对应的 UI 槽位
        for (int i = 0; i < stash.Count; i++)
            stashItemSlot[i].UpdateSlot(stash[i]);

        // 装备槽位根据类型填充
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (equipmentSlot[i].slotType == item.Key.equipmentType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }
    }

    // 添加物品，根据物品类型决定放背包还是仓库
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);                                                  // 装备 → 背包
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);                                                      // 材料 → 仓库

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }

    // 添加物品到背包
    private void AddToInventory(ItemData _item)
    {
        // 如果物品已经存在字典中，则数量 +1
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // 不存在，则创建新物品实例，加入背包
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    // 添加物品到仓库
    private void AddToStash(ItemData _item)
    {
        // 如果物品已经存在字典中，则数量 +1
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // 不存在，则创建新物品实例，加入仓库
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    // 根据物品类型移除物品
    public void RemoveItem(ItemData _item)
    {
        // 如果背包里有这个物品
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                // 如果只剩最后一个，就从列表和字典中删除
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();                                                // 否则只减少数量
        }

        // 如果仓库里有这个物品
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                // 如果只剩最后一个，就从列表和字典中删除
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();                                           // 否则只减少数量
        }

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }
}
