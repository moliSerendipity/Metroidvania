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

        ItemData_Equipment OldEquipment = null;

        // �ҵ�ͬ���͵ľ�װ��������/���׵ȣ�
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                OldEquipment = item.Key;
        }

        // ����о�װ����ж�²��Żر���
        if (OldEquipment != null)
        {
            if (equipmentDictionary.TryGetValue(OldEquipment, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(OldEquipment);
            }

            AddItem(OldEquipment);                                                  // ж�µ�װ���Żر���
        }

        // �����װ��
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);

        RemoveItem(_item);                                                          // �ӱ����Ƴ���װ������Ʒ
        //UpdateSlotUI();
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

    // �����Ʒ��������Ʒ���;����ű������ǲֿ�
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);                                                  // װ�� �� ����
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);                                                      // ���� �� �ֿ�

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }

    // �����Ʒ������
    private void AddToInventory(ItemData _item)
    {
        // �����Ʒ�Ѿ������ֵ��У������� +1
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // �����ڣ��򴴽�����Ʒʵ�������뱳��
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    // �����Ʒ���ֿ�
    private void AddToStash(ItemData _item)
    {
        // �����Ʒ�Ѿ������ֵ��У������� +1
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // �����ڣ��򴴽�����Ʒʵ��������ֿ�
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    // ������Ʒ�����Ƴ���Ʒ
    public void RemoveItem(ItemData _item)
    {
        // ����������������Ʒ
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                // ���ֻʣ���һ�����ʹ��б���ֵ���ɾ��
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();                                                // ����ֻ��������
        }

        // ����ֿ����������Ʒ
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                // ���ֻʣ���һ�����ʹ��б���ֵ���ɾ��
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();                                           // ����ֻ��������
        }

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }
}
