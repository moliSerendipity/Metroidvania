using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 玩家碰到物体时拾取
        if (collision.GetComponent<Player>())
        {
            myItemObject.PickUpItem();
        }
    }
}
