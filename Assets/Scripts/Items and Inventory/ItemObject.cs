using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景中的物品实体
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;                                 // 该物体绑定的数据（决定图标/名字/类型等）

    private void OnValidate()
    {
        // 在编辑器里自动更新物体外观
        GetComponent<SpriteRenderer>().sprite = itemData.icon;                  // 显示物品图标
        gameObject.name = "Item object - " + itemData.itemName;                 // 自动改名方便调试
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 玩家碰到物体时拾取
        if (collision.GetComponent<Player>())
        {
            Inventory.instance.AddItem(itemData);                               // 加入背包
            Destroy(gameObject);                                                // 销毁场景里的物体
        }
    }
}
