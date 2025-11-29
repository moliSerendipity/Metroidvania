using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

// 数值、buff需要重新设计
public class EntityStats : MonoBehaviour
{
    private EntityFX fx;

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
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = 0.3f;
    private float igniteDamageTimer;
    private float igniteDamage;                                                     // 受到的点燃伤害

    [SerializeField] private GameObject shockStrikePrefab;
    private float shockDamage;

    [SerializeField] private float ailmentDuration = 4;

    public float currentHealth;                                                     // 当前生命值

    public System.Action onHealthChanged;                                           // 
    public bool isDead { get; private set; }
    private bool isVulnerable;

    private void Awake()
    {
    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();

        critDamage.SetDefaultValue(1.5f);
        currentHealth = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
            isIgnited = false;
        if(isIgnited && igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
                Die();
            igniteDamageTimer = igniteDamageCooldown;
        }

        chilledTimer -= Time.deltaTime;
        if (chilledTimer < 0)
            isChilled = false;

        shockedTimer -= Time.deltaTime;
        if (shockedTimer < 0)
            isShocked = false;
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCoroutine(_duration));
    }

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    /// <summary>
    /// 增加buff
    /// </summary>
    /// <param name="_statToModify">加成属性</param>
    /// <param name="_modifier">加成数值</param>
    /// <param name="_duration">buff持续时间</param>
    public virtual void IncreaseStatBy(Stat _statToModify, float _modifier,float _duration)
    {
        StartCoroutine(StatModCoroutine(_statToModify, _modifier, _duration));
    }

    private IEnumerator StatModCoroutine(Stat _statToModify, float _modifier, float _duration)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    // 获取生命上限
    public float GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

    public virtual void OnEvasion()
    {

    }

    // 是否能闪避攻击
    protected bool TargetCanAvoidAttack(EntityStats _targetStats)
    {
        // 总闪避率
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            Debug.Log("Attack avoided");
            return true;
        }
        return false;
    }

    // 能否暴击
    protected bool CanCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;
        return false;
    }

    // 暴击伤害
    protected float CalculateCriticalDamage(float _damage)
    {
         return _damage * (critDamage.GetValue() + strength.GetValue());
    }

    // 恢复生命值
    public virtual void IncreaseHealthBy(float _amount)
    {
        currentHealth += _amount;
        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        onHealthChanged?.Invoke();                                                  // 血条 UI 更新
    }

    // 减少血量
    protected virtual void DecreaseHealthBy(float _damage)
    {
        if (isVulnerable)
            _damage *= 1.2f;

        currentHealth -= _damage;
        onHealthChanged?.Invoke();                                                  // 血条 UI 更新
    }

    // 受到伤害
    public virtual void TakeDamage(float _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        Debug.Log(_damage);

        if (currentHealth <= 0 && !isDead)                                                     // 如果血量<=0就死亡
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

        DoMagicDamage(_targetStats);
    }

    // 攻击，对目标造成法伤
    public virtual void DoMagicDamage(EntityStats _targetStats)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();

        float totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage *= (1 - _targetStats.magicResistance.GetValue());
        _targetStats.TakeDamage(totalMagicDamage);

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(_fireDamage * 0.2f);

        if (canApplyShock)
            _targetStats.SetupSHockStrikeDamage(_lightningDamage * 0.1f);
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
            fx.IgniteFXFor(4);
        }
        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = 4;
            GetComponent<Entity>().SlowEntityBy(0.2f, 4);
            fx.ChillFXFor(4);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (!GetComponent<Player>())
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        isShocked = _shock;
        shockedTimer = 4;
        fx.ShockFXFor(4);
    }

    private void HitNearestTargetWithShockStrike()
    {
        // 获取附近所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        float closestDistance = Mathf.Infinity;                                     // 最近距离，初始值无穷大
        Transform closestEnemy = null;                                              // 最近敌人的坐标
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                // 如果检测到敌人，获取最近敌人的位置和距离
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<EntityStats>());
        }
    }

    public void SetupIgniteDamage(float _igniteDamage) => igniteDamage = _igniteDamage;

    public void SetupSHockStrikeDamage(float _shockStrikeDamage) => shockDamage = _shockStrikeDamage;

    // 死亡
    protected virtual void Die()
    {
        isDead = true;
    }

    public Stat GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intellience:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critDamage:
                return critDamage;
            case StatType.health:
                return maxHealth;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResistance:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightningDamage;
            default:
                return null;
        }
    }
}
