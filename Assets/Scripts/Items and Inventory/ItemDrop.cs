using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������������Ʒ���߼�
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfPossibleDropItems;                         // ��������Ʒ����
    [SerializeField] private ItemData[] possibleDrop;                               // ��������ܵ������Ʒ��
    private List<ItemData> dropList = new List<ItemData>();                         // ����ʵ�ʿɵ������Ʒ��

    [SerializeField] private GameObject dropPrefab;                                 // ���������Ԥ���壨ItemObject��
    //[SerializeField] private ItemData item;

    // ���ɵ���
    public void GenerateDrop()
    {
        // ��������������ʾ����Ƿ�����ѡ�б�
        for (int i = 0;  i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)                 // ��������ж�
                dropList.Add(possibleDrop[i]);
        }

        // ����к�ѡ��Ʒ���������ѡ N ��
        if (dropList.Count > 0)
            for (int i = 0; i < amountOfPossibleDropItems; i++)
            {
                if (dropList.Count <= 0)
                    break;

                ItemData randomItem = dropList[Random.Range(0, dropList.Count)];    // ���ѡһ��
                dropList.Remove(randomItem);                                        // �Ƴ��������ظ�
                DropItem(randomItem);                                               // ���ɵ�������
            }
    }

    // ʵ�����ɵ�������
    public void DropItem(ItemData _itemData)
    {
        // ʵ��������
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // ������һ��������ٶ�
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        // ������Ʒ���ݲ�Ӧ��
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
