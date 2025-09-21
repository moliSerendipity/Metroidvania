using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;
    [SerializeField] protected Image itemImage;                                 // ��λ����ʾ��ͼƬ����Ʒͼ�꣩
    [SerializeField] protected TextMeshProUGUI itemText;                        // ��λ���½ǵ����֣���Ʒ������

    public InventoryItem item;                                                  // ��ǰ��λ�󶨵���Ʒ������Ϊ�գ�

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

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
        if (item == null)
            return;

        // ��ס LeftControl ������Ʒ
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        // װ����Ʒ
        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        if (item != null)
            ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        else
            ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.HideToolTip();
    }
}
