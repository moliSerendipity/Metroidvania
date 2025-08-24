using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 怪物死亡后掉落物品的逻辑
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfPossibleDropItems;                         // 最大掉落物品数量
    [SerializeField] private ItemData[] possibleDrop;                               // 掉落表（可能掉落的物品）
    private List<ItemData> dropList = new List<ItemData>();                         // 本次实际可掉落的物品池

    [SerializeField] private GameObject dropPrefab;                                 // 掉落物体的预制体（ItemObject）
    //[SerializeField] private ItemData item;

    // 生成掉落
    public void GenerateDrop()
    {
        // 遍历掉落表，按概率决定是否进入候选列表
        for (int i = 0;  i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)                 // 掉落概率判断
                dropList.Add(possibleDrop[i]);
        }

        // 如果有候选物品，则随机挑选 N 个
        if (dropList.Count > 0)
            for (int i = 0; i < amountOfPossibleDropItems; i++)
            {
                if (dropList.Count <= 0)
                    break;

                ItemData randomItem = dropList[Random.Range(0, dropList.Count)];    // 随机选一个
                dropList.Remove(randomItem);                                        // 移除，避免重复
                DropItem(randomItem);                                               // 生成掉落物体
            }
    }

    // 实际生成掉落物体
    public void DropItem(ItemData _itemData)
    {
        // 实例化物体
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        // 给物体一个随机初速度
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        // 设置物品数据并应用
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
