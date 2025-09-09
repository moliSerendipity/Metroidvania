using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����е������Ʒʵ�壨�ɽ����� Item��
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;                                 // ������󶨵����ݣ�����ͼ��/����/���͵ȣ�

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    // ������Ʒ����ۣ�ͼ��/���֣�
    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;                  // ������Ʒͼ��
        gameObject.name = "Item object - " + itemData.itemName;                 // �ڲ㼶������Զ�����
    }

    // ������Ʒ����
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;                                                   // ����Ʒ����
        rb.velocity = _velocity;                                                // ���ó�ʼ�ٶȣ�����ɢ��Ч����
        SetupVisuals();                                                         // ˢ�����
    }

    // ���ʰȡ��Ʒʱ����
    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 5);
            return;
        }

        Inventory.instance.AddItem(itemData);                                   // ���뱳��
        Destroy(gameObject);                                                    // ���ٳ����������
    }
}
