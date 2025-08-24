using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ����ϵͳ�еĲ�λ���̳��� UI_ItemSlot����������ʾҪ������װ�������������������߼�
public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);                                                           // �� UI ����ʱ��ˢ��һ����ʾ
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;             // ��ǰ��λ�󶨵�װ��
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);        // ���ñ����������߼�
    }
}
