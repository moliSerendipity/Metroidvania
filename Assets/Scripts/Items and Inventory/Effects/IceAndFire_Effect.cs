using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 冰火弹效果（继承自 ItemEffect）
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;                           // 冰火弹预制体
    [SerializeField] private Vector2 newVelocity;                                   // 发射速度

    // 在敌人位置生成冰火弹，并赋予初始速度
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Transform player = PlayerManager.instance.player.transform;
        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.rotation);
        newIceAndFire.GetComponent<Rigidbody2D>().velocity = newVelocity;
    }
}
