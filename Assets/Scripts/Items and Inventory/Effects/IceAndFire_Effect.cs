using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 冰火弹效果（继承自 ItemEffect）
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;                           // 冰火弹预制体
    [SerializeField] private Vector2 newVelocity;                                   // 发射速度

    // 第三段攻击时，在敌人位置生成冰火弹，并赋予初始速度，并在5秒后销毁
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;
        // 是否是第三段攻击
        bool thirdAttack = player.primaryAttack.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = newVelocity * player.facingDir;
            Destroy(newIceAndFire, 5);
        }
    }
}
