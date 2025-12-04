using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景中掉落的物品实体（可交互的 Item）
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;                                 // 该物体绑定的数据（决定图标/名字/类型等）

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
    }

    // 设置物品的外观（图标/名字）
    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;                  // 设置物品图标
        gameObject.name = "Item object - " + itemData.itemName;                 // 在层级面板里自动改名
    }

    // 设置物品参数
    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;                                                   // 绑定物品数据
        rb.velocity = _velocity;                                                // 设置初始速度（掉落散射效果）
        SetupVisuals();                                                         // 刷新外观
    }

    // 玩家拾取物品时调用
    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 5);
            PlayerManager.instance.player.fx.CreatePopUpText("Inventory is full");
            return;
        }

        AudioManager.instance.PlaySFX(18);
        Inventory.instance.AddItem(itemData);                                   // 加入背包
        Destroy(gameObject);                                                    // 销毁场景里的物体
    }
}
