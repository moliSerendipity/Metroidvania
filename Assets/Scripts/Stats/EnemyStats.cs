using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;                                         // ���˵ȼ�

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critDamage);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1;i < level;i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(modifier);
        }
    }

    // ���ˣ���ʾ����Ч��
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //enemy.DamageEffect();                                                     // ��ʾ����Ч��
    }

    // ��������������״̬
    protected override void Die()
    {
        base.Die();

        enemy.Die();                                                                // ��������״̬
        myDropSystem.GenerateDrop();                                                // ������Ʒ
    }
}
