using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;

    [Header("Offensive stats")]
    public Stat damage;                                                             // 攻击力
    public Stat critChance;                                                         // 暴击率
    public Stat critDamage;                                                         // 暴击伤害

    [Header("Defensive stats")]
    public Stat maxHealth;                                                          // 最大生命值
    public Stat armor;                                                              // 护甲值
    public Stat evasion;                                                            // 闪避率

    [SerializeField] private float currentHealth;                                   // 当前生命值

    protected virtual void Start()
    {
        critDamage.SetDefaultValue(1.5f);
        currentHealth = maxHealth.GetValue();
    }

    // 是否能闪避攻击
    private bool TargetCanAvoidAttack(EntityStats _targetStats)
    {
        // 总闪避率
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoided");
            return true;
        }
        return false;
    }

    // 能否暴击
    private bool CanCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;
        return false;
    }

    // 暴击伤害
    private float CalculateCriticalDamage(float _damage)
    {
         return _damage * (critDamage.GetValue() + strength.GetValue());
    }

    // 受到伤害
    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);

        if (currentHealth <= 0)                                                     // 如果血量<=0就死亡
            Die();
    }

    // 攻击，对目标造成伤害
    public virtual void DoDamage(EntityStats _targetStats)
    {
        // 是否能闪避攻击
        if (TargetCanAvoidAttack(_targetStats))
            return;

        float totalDamage = damage.GetValue() + strength.GetValue();                  // 总伤害值
        // 如果暴击，总伤害值改成暴击后的伤害
        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);
        // 总伤害值如果低于被攻击对象护甲，则只造成5%的伤害，否则最终伤害为总伤害值-被攻击对象护甲值
        totalDamage = (totalDamage > _targetStats.armor.GetValue()) ? totalDamage - _targetStats.armor.GetValue() : totalDamage * 0.05f;
        _targetStats.TakeDamage(totalDamage);                                       // 被攻击对象受伤
    }

    // 死亡
    protected virtual void Die()
    {
        
    }
}
