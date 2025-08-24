using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������ࣨ������
public class Inventory : MonoBehaviour
{
    public static Inventory instance;                                               // ����������ȫ�ֵ���

    public List<InventoryItem> equipment;                                           // ��װ����Ʒ�б�
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;       // װ���ֵ䣨�����Ϳ��ٲ��ң�

    public List<InventoryItem> inventory;                                           // ������Ʒ�б�
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;                 // ���ٲ��ң�Key = ItemData��

    public List<InventoryItem> stash;                                               // �ֿ���Ʒ�б�
    public Dictionary <ItemData, InventoryItem> stashDictionary;                    // �ֿ��ֵ�

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;                         // ���� UI ��λ������
    [SerializeField] private Transform stashSlotParent;                             // �ֿ� UI ��λ������
    [SerializeField] private Transform equipmentSlotParent;                         // װ�� UI ��λ������

    private UI_ItemSlot[] inventoryItemSlot;                                        // ������λ
    private UI_ItemSlot[] stashItemSlot;                                            // �ֿ��λ
    private UI_EquipmentSlot[] equipmentSlot;                                       // װ����λ

    private void Awake()
    {
        // ����ģʽ��ȷ��ֻ��һ�� Inventory
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // ��ʼ�������б���ֵ�
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        // ��ȡ UI ��λ���������µ����в�λ�ű���
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    // װ����Ʒ�߼�
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;              // ǿ��ת�ͣ���װ���᷵�� null��
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        // �ҵ�ͬ���͵ľ�װ��������/���׵ȣ�
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        // ����о�װ����ж�²��Żر���
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);                                              // ж��װ��
            AddItem(oldEquipment);                                                  // ж�µ�װ���Żر���
        }

        // �����װ��
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();                                                // װ�������ӽ�ɫ��ֵ

        RemoveItem(_item);                                                          // �ӱ����Ƴ���װ������Ʒ
        //UpdateSlotUI();
    }

    // ж��װ��
    public void UnequipItem(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();                                         // ж�º���ٽ�ɫ��ֵ
        }
    }

    // ���� UI ��λ��ʾ
    private void UpdateSlotUI()
    {
        // ��������в�λ
        for (int i = 0; i < inventoryItemSlot.Length; i++)
            inventoryItemSlot[i].CleanUpSlot();
        for (int i = 0; i < stashItemSlot.Length; i++)
            stashItemSlot[i].CleanUpSlot();

        // ��һ��䱳�� & �ֿ��λ
        for (int i = 0; i < inventory.Count; i++)
            inventoryItemSlot[i].UpdateSlot(inventory[i]);                          // ����Ʒ��ʾ����Ӧ�� UI ��λ
        for (int i = 0; i < stash.Count; i++)
            stashItemSlot[i].UpdateSlot(stash[i]);

        // װ����λ�����������
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
    /// ���ָ��������Ʒ��������Ʒ���;����ű������ǲֿ�
    /// </summary>
    /// <param name="_item">Ҫ��ӵ���Ʒ</param>
    /// <param name="amount">Ҫ��ӵ�����</param>
    public void AddItem(ItemData _item, int amount = 1)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item, amount);                                                  // װ�� �� ����
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item, amount);                                                      // ���� �� �ֿ�

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }

    /// <summary>
    /// ���ָ��������Ʒ������
    /// </summary>
    /// <param name="_item">Ҫ��ӵ���Ʒ</param>
    /// <param name="amount">Ҫ��ӵ�����</param>
    private void AddToInventory(ItemData _item, int amount = 1)
    {
        // �����Ʒ�Ѿ������ֵ��У������� +amount
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack(amount);
        else
        {
            // �����ڣ��򴴽�����Ʒʵ�������뱳��
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = amount;                                             // ��ʼ������ = amount
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// ���ָ��������Ʒ���ֿ�
    /// </summary>
    /// <param name="_item">Ҫ��ӵ���Ʒ</param>
    /// <param name="amount">Ҫ��ӵ�����</param>
    private void AddToStash(ItemData _item, int amount = 1)
    {
        // �����Ʒ�Ѿ������ֵ��У������� +amount
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack(amount);
        else
        {
            // �����ڣ��򴴽�����Ʒʵ��������ֿ�
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = amount;                                             // ��ʼ������ = amount
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// �Ƴ�ָ��������Ʒ
    /// </summary>
    /// <param name="_item">��Ҫ�Ƴ�����Ʒ</param>
    /// <param name="amount">�Ƴ�������</param>
    public void RemoveItem(ItemData _item, int amount = 1)
    {
        // ����������������Ʒ
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.RemoveStack(amount);                                              // ��ȥ����
            if (value.stackSize <= 0)                                               // ���û��
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
        }

        // ����ֿ����������Ʒ
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            stashValue.RemoveStack(amount);                                         // ��ȥ����
            if (stashValue.stackSize <= 0)                                          // ���û��
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
        }

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }

    /// <summary>
    /// �ж�����Ƿ�������ָ����װ�������ڳɹ�ʱ���Ĳ��ϲ�����װ��
    /// </summary>
    /// <param name="_itemToCraft">Ҫ�����װ�������ղ��</param>
    /// <param name="_requiredMaterials">��������Ĳ��ϼ�������InventoryItem ��ʽ��</param>
    /// <returns>�ܷ�����ɹ�</returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        // ����������������в���
        foreach (InventoryItem req in _requiredMaterials)
        {
            // �����ڻ���������
            if (!stashDictionary.TryGetValue(req.data, out InventoryItem stashValue) || stashValue.stackSize < req.stackSize)
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        // �����㹻 �� �۳�����
        foreach (InventoryItem req in _requiredMaterials)
            RemoveItem(req.data, req.stackSize);                                    // һ�μ�����Ҫ������

        AddItem(_itemToCraft);                                                      // ���Ͽ۳���ϣ���������µ�װ��
        Debug.Log("Here is your item " +  _itemToCraft.name);
        return true;                                                                // ����ɹ�
    }
}
