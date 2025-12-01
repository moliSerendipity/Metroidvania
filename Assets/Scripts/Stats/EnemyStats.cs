using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int level = 1;                                         // 敌人等级

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
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

        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1;i < level;i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(modifier);
        }
    }

    // 受伤，显示受伤效果
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //enemy.DamageEffect();                                                     // 显示受伤效果
    }

    // 死亡，进入死亡状态
    protected override void Die()
    {
        base.Die();

        enemy.Die();                                                                // 进入死亡状态
        PlayerManager.instance.currency += (int)soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();                                                // 掉落物品
        Destroy(gameObject, 5f);                                                    // 5秒后销毁敌人对象
    }
}
