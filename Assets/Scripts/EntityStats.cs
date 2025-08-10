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
    public Stat damage;                                                             // ������
    public Stat critChance;                                                         // ������
    public Stat critDamage;                                                         // �����˺�

    [Header("Defensive stats")]
    public Stat maxHealth;                                                          // �������ֵ
    public Stat armor;                                                              // ����ֵ
    public Stat evasion;                                                            // ������

    [SerializeField] private float currentHealth;                                   // ��ǰ����ֵ

    protected virtual void Start()
    {
        critDamage.SetDefaultValue(1.5f);
        currentHealth = maxHealth.GetValue();
    }

    // �Ƿ������ܹ���
    private bool TargetCanAvoidAttack(EntityStats _targetStats)
    {
        // ��������
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Attack avoided");
            return true;
        }
        return false;
    }

    // �ܷ񱩻�
    private bool CanCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;
        return false;
    }

    // �����˺�
    private float CalculateCriticalDamage(float _damage)
    {
         return _damage * (critDamage.GetValue() + strength.GetValue());
    }

    // �ܵ��˺�
    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);

        if (currentHealth <= 0)                                                     // ���Ѫ��<=0������
            Die();
    }

    // ��������Ŀ������˺�
    public virtual void DoDamage(EntityStats _targetStats)
    {
        // �Ƿ������ܹ���
        if (TargetCanAvoidAttack(_targetStats))
            return;

        float totalDamage = damage.GetValue() + strength.GetValue();                  // ���˺�ֵ
        // ������������˺�ֵ�ĳɱ�������˺�
        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);
        // ���˺�ֵ������ڱ��������󻤼ף���ֻ���5%���˺������������˺�Ϊ���˺�ֵ-���������󻤼�ֵ
        totalDamage = (totalDamage > _targetStats.armor.GetValue()) ? totalDamage - _targetStats.armor.GetValue() : totalDamage * 0.05f;
        _targetStats.TakeDamage(totalDamage);                                       // ��������������
    }

    // ����
    protected virtual void Die()
    {
        
    }
}
