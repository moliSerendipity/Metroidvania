using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;                                   // ��λ����ʾ��ͼƬ����Ʒͼ�꣩
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
}
