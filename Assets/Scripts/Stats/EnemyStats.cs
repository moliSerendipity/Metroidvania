using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    // 受伤，显示受伤效果
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //enemy.DamageEffect();                                                       // 显示受伤效果
    }

    // 死亡，进入死亡状态
    protected override void Die()
    {
        base.Die();
        enemy.Die();                                                                // 进入死亡状态
    }
}
