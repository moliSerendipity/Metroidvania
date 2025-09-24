using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;                             // ����������� CraftSlot �ĸ����壨������
    [SerializeField] private GameObject craftSlotPrefab;                            // �ϳɲ�Ԥ����
    [SerializeField] private List<ItemData_Equipment> craftEquipment;               // �ɺϳɵ�װ���б�

    void Start()
    {   // �Լ��ڸ������е������� 0����һ����
        if (transform.GetSiblingIndex() == 0) 
        {
            SetupCraftList();                                                       // ˢ�ºϳ��б� UI
            SetupDefaultCraftWindow();                                              // Ĭ����ʾ��һ����Ʒ�ĺϳ�����
        }
    }

    /// <summary>
    /// ˢ�ºϳ��б� UI���� craftEquipment �����Ʒ���ɵ� CraftSlot ��
    /// </summary>
    public void SetupCraftList()
    {
        // ��վɵĲ�λ�������ظ����ɣ�
        for (int i = 0; i < craftSlotParent.childCount; i++)
            Destroy(craftSlotParent.GetChild(i).gameObject);

        // ����װ���б�ÿ��װ������һ�� CraftSlot
        for (int i = 0;i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);     // �����²�λ
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]); // ������ʾ����
        }
    }

    /// <summary>
    /// Ĭ����ʾ��һ����Ʒ�ĺϳ�����
    /// </summary>
    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }

    /// <summary>
    /// ����� CraftList ʱ����
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();                                                           // ˢ�ºϳ��б�
        SetupDefaultCraftWindow();                                                  // ��ʾĬ�Ϻϳ�����
    }
}