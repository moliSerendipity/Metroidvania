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

        ItemData_Equipment oldEquipment = null;

        // 找到同类型的旧装备（武器/护甲等）
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        // 如果有旧装备，卸下并放回背包
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);                                              // 卸下装备
            AddItem(oldEquipment);                                                  // 卸下的装备放回背包
        }

        // 添加新装备
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();                                                // 装备后增加角色数值

        RemoveItem(_item);                                                          // 从背包移除刚装备的物品
        //UpdateSlotUI();
    }

    // 卸下装备
    public void UnequipItem(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();                                         // 卸下后减少角色数值
        }
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

    /// <summary>
    /// 添加指定数量物品，根据物品类型决定放背包还是仓库
    /// </summary>
    /// <param name="_item">要添加的物品</param>
    /// <param name="amount">要添加的数量</param>
    public void AddItem(ItemData _item, int amount = 1)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item, amount);                                                  // 装备 → 背包
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item, amount);                                                      // 材料 → 仓库

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }

    /// <summary>
    /// 添加指定数量物品到背包
    /// </summary>
    /// <param name="_item">要添加的物品</param>
    /// <param name="amount">要添加的数量</param>
    private void AddToInventory(ItemData _item, int amount = 1)
    {
        // 如果物品已经存在字典中，则数量 +amount
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack(amount);
        else
        {
            // 不存在，则创建新物品实例，加入背包
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = amount;                                             // 初始化数量 = amount
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 添加指定数量物品到仓库
    /// </summary>
    /// <param name="_item">要添加的物品</param>
    /// <param name="amount">要添加的数量</param>
    private void AddToStash(ItemData _item, int amount = 1)
    {
        // 如果物品已经存在字典中，则数量 +amount
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack(amount);
        else
        {
            // 不存在，则创建新物品实例，加入仓库
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = amount;                                             // 初始化数量 = amount
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 移除指定数量物品
    /// </summary>
    /// <param name="_item">需要移除的物品</param>
    /// <param name="amount">移除的数量</param>
    public void RemoveItem(ItemData _item, int amount = 1)
    {
        // 如果背包里有这个物品
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.RemoveStack(amount);                                              // 减去数量
            if (value.stackSize <= 0)                                               // 如果没了
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
        }

        // 如果仓库里有这个物品
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            stashValue.RemoveStack(amount);                                         // 减去数量
            if (stashValue.stackSize <= 0)                                          // 如果没了
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
        }

        UpdateSlotUI();                                                             // 每次修改后刷新 UI
    }

    /// <summary>
    /// 判断玩家是否能制造指定的装备，并在成功时消耗材料并生成装备
    /// </summary>
    /// <param name="_itemToCraft">要制造的装备（最终产物）</param>
    /// <param name="_requiredMaterials">制造所需的材料及数量（InventoryItem 形式）</param>
    /// <returns>能否制造成功</returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        // 遍历制造所需的所有材料
        foreach (InventoryItem req in _requiredMaterials)
        {
            // 不存在或数量不足
            if (!stashDictionary.TryGetValue(req.data, out InventoryItem stashValue) || stashValue.stackSize < req.stackSize)
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        // 材料足够 → 扣除数量
        foreach (InventoryItem req in _requiredMaterials)
            RemoveItem(req.data, req.stackSize);                                    // 一次减掉需要的数量

        AddItem(_itemToCraft);                                                      // 材料扣除完毕，给予玩家新的装备
        Debug.Log("Here is your item " +  _itemToCraft.name);
        return true;                                                                // 制造成功
    }
}
