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

    // 减少血量，并触发护甲效果
    protected override void DecreaseHealthBy(float _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (_damage > GetMaxHealthValue() * 0.3f)
        {
            player.SetupKnockbackPower(new Vector2(10, 6));
            int randomSound = Random.Range(34,36);
            AudioManager.instance.PlaySFX(randomSound);
        }

        Inventory.instance.GetEquipment(EquipmentType.Armor)?.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(EntityStats _targetStats, float _multiplier)
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
        if (_multiplier > 0) totalDamage *= _multiplier;
        _targetStats.TakeDamage(totalDamage);                                       // 被攻击对象受伤

        DoMagicDamage(_targetStats);
    }

    // 死亡，进入死亡状态
    protected override void Die()
    {
        base.Die();

        player.Die();                                                               // 进入死亡状态
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        GetComponent<PlayerItemDrop>()?.GenerateDrop();                             // 掉落物品
    }
}
