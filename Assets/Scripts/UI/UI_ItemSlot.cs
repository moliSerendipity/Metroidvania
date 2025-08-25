using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image itemImage;                                 // ��λ����ʾ��ͼƬ����Ʒͼ�꣩
    [SerializeField] private TextMeshProUGUI itemText;                          // ��λ���½ǵ����֣���Ʒ������

    public InventoryItem item;                                                  // ��ǰ��λ�󶨵���Ʒ������Ϊ�գ�

    // ���²�λ����ʾ����
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;                                                        // �󶨵���ǰ����
        if (item != null)
        {
            // ��ʾͼ��
            itemImage.color = Color.white;                                      // ��ͼƬ��ɫ�ָ�������������͸��״̬��
            itemImage.sprite = item.data.icon;                                  // ������Ʒͼ��

            // ��ʾ������������� 1 ����ʾ���֣�
            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";                                             // ������Ʒ�Ͳ���ʾ����
        }
        else
        {
            itemImage.color = Color.clear;                                      // ��ͼƬ����
            itemImage.sprite = null;                                            // ���ͼ��
            itemText.text = "";                                                 // �������
        }
    }

    // �ֶ���ո���
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    // �������ʱ�����߼�
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // ��ס LeftControl ������Ʒ
        if (item != null && Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        // װ����Ʒ
        if (item != null && item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }
}
