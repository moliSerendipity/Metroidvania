using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// װ����λ���̳��� UI_ItemSlot����������ʾװ�������Ҵ�����ж�µ��߼�
public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;                                                  // ��λ���ͣ�����/����/����/ҩƿ��

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    // ����¼�����д UI_ItemSlot �ĵ���߼���
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null || itemImage.sprite == null)          // û��װ����ֱ�ӷ���
            return;

        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);            // �ѵ�ǰװ��ж��
        Inventory.instance.AddItem(item.data as ItemData_Equipment);                // ��ж�µ�װ�����·Żر���
        ui.itemToolTip.HideToolTip();                                               // ������Ʒ��ʾ��
        CleanUpSlot();                                                              // ��ղ�λ
    }
}
