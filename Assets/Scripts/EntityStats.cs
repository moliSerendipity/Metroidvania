using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;                                                             // 攻击力
    public Stat maxHealth;                                                          // 最大生命值
    [SerializeField] private int currentHealth;                                     // 当前生命值

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    // 受到伤害
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);

        if (currentHealth <= 0)
            Die();
    }

    public virtual void DoDamage(EntityStats _targetStats)
    {


        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    // 死亡
    protected virtual void Die()
    {
        throw new NotImplementedException();
    }
}
