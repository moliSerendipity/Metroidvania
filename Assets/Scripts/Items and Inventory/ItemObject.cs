using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����е���Ʒʵ��
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;                                 // ������󶨵����ݣ�����ͼ��/����/���͵ȣ�

    private void OnValidate()
    {
        // �ڱ༭�����Զ������������
        GetComponent<SpriteRenderer>().sprite = itemData.icon;                  // ��ʾ��Ʒͼ��
        gameObject.name = "Item object - " + itemData.itemName;                 // �Զ������������
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������ʱʰȡ
        if (collision.GetComponent<Player>())
        {
            Inventory.instance.AddItem(itemData);                               // ���뱳��
            Destroy(gameObject);                                                // ���ٳ����������
        }
    }
}
