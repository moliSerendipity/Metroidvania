using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������ࣨ������
public class Inventory : MonoBehaviour
{
    public static Inventory instance;                                               // ����������ȫ�ֵ���
    public List<InventoryItem> inventoryItems;                                      // ������Ʒ�б�
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;                 // ���ٲ��ң�Key = ItemData��

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;                         // UI��λ�ĸ�����

    private UI_ItemSlot[] itemSlot;                                                 // ��λ���飨��Ӧ UI��

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
        inventoryItems = new List<InventoryItem>();                                 // ��ʼ����Ʒ�б�
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();            // ��ʼ���ֵ�

        // ��ȡ UI ��λ���������µ����в�λ�ű���
        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    // ���� UI ��λ��ʾ
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
            itemSlot[i].UpdateSlot(inventoryItems[i]);                              // ����Ʒ��ʾ����Ӧ�� UI ��λ
    }

    // �����Ʒ
    public void AddItem(ItemData _item)
    {
        // �����Ʒ�Ѿ������ֵ��У������� +1
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            // �����ڣ��򴴽�����Ʒʵ�������뱳��
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }

    public void RemoveItem(ItemData _item)
    {
        // ����������������Ʒ
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                // ���ֻʣ���һ�����ʹ��б���ֵ���ɾ��
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();                                                // ����ֻ��������
        }

        UpdateSlotUI();                                                             // ÿ���޸ĺ�ˢ�� UI
    }
}
