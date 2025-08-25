using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    // 受伤，显示受伤效果
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //player.DamageEffect();                                                      // 显示受伤效果
    }

    // 死亡，进入死亡状态
    protected override void Die()
    {
        base.Die();

        player.Die();                                                               // 进入死亡状态
        GetComponent<PlayerItemDrop>()?.GenerateDrop();                             // 掉落物品
    }
}
