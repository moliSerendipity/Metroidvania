using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// UI װ����λ���߼���������ʾ��Ʒͼ�ꡢ����������Ӧ�������ͣ���¼�
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;                                                            // UI ������
    [SerializeField] protected Image itemImage;                                 // ��λ����ʾ��ͼƬ����Ʒͼ�꣩
    [SerializeField] protected TextMeshProUGUI itemText;                        // ��λ���½ǵ����֣���Ʒ������

    public InventoryItem item;                                                  // ��ǰ��λ�󶨵���Ʒ������Ϊ�գ�

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    /// <summary>
    /// ���²�λ��ʾ����
    /// </summary>
    /// <param name="_newItem">Ҫ�󶨵�����Ʒ������Ϊ��</param>
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
            // �����ʾ
            itemImage.color = Color.clear;                                      // ͼ��͸��
            itemImage.sprite = null;                                            // ���ͼ��
            itemText.text = "";                                                 // �������
        }
    }

    /// <summary>
    /// �ֶ���ո��ӣ������Ƴ���Ʒ��
    /// </summary>
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    /// <summary>
    /// �������ʱ���߼�
    /// </summary>
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

        // �����װ����ֱ��װ��
        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        // ��ʾ�����ǰ��������Ʒ����ʾ�򣬻򵱸���������Ʒʱ������ʾ��
        if (item != null)
            ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        else
            ui.itemToolTip.HideToolTip();
    }

    /// <summary>
    /// ���������ӣ���ʾ��ʾ��
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    /// <summary>
    /// ����Ƴ����ӣ�������ʾ��
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;

        ui.itemToolTip.HideToolTip();
    }
}