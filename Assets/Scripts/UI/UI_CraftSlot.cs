using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ����ϵͳ�еĲ�λ���̳��� UI_ItemSlot����������ʾҪ������װ�������������������߼�
public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// ���úϳɲ۵���ʾ����
    /// </summary>
    /// <param name="_data">Ҫչʾ��װ������</param>
    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }

    /// <summary>
    /// ������ϳɲ�ʱ�����߼�
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        // ���ʱ���Ѹ�װ�������ݴ��ݸ��ϳɴ��ڣ����� UI
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
