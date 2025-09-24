using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    // UI ����󶨣���Ʒ���ơ���Ʒ���͡���Ʒ����
    [SerializeField] private TextMeshProUGUI itemNameText;                          // ��ʾ��Ʒ����
    [SerializeField] private TextMeshProUGUI itemTypeText;                          // ��ʾ��Ʒ���ͣ��������������ߣ�
    [SerializeField] private TextMeshProUGUI itemDescription;                       // ��ʾ��Ʒ����

    void Start()
    {
        
    }

    /// <summary>
    /// ��ʾ��Ʒ��ʾ��
    /// </summary>
    /// <param name="item">Ҫչʾ����Ʒ��װ�������ݣ�</param>
    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null) return;

        // ���� UI ����
        itemNameText.text = item.itemName;                                          // ��������
        itemTypeText.text = item.equipmentType.ToString();                          // �������ͣ�ö��ת�ַ�����
        itemDescription.text = item.GetDescription();                               // ����������������Ʒ�����ݷ�����

        gameObject.SetActive(true);                                                 // ������ʾ��������ʾ�ڻ�����
    }

    /// <summary>
    /// ������Ʒ��ʾ��
    /// </summary>
    public void HideToolTip() => gameObject.SetActive(false);                       // ֱ�ӽ��� GameObject
}
