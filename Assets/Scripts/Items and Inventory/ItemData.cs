using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ʒ���ͣ����ࣩ
public enum ItemType
{
    Material,                                                                       // ��������Ʒ
    Equipment                                                                       // װ������Ʒ
}

// ͨ����Ʒ���ݻ��࣬ʹ�� ScriptableObject ����������ģ��
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;                                                       // ��Ʒ����
    public string itemName;                                                         // ��Ʒ����
    public Sprite icon;                                                             // ��Ʒͼ��

    [Range(0, 100)]
    public float dropChance;                                                        // �������
}
