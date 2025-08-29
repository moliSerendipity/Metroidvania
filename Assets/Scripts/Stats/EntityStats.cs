using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

// ��ֵ��buff��Ҫ�������
public class EntityStats : MonoBehaviour
{
    private EntityFX fx;

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
    private float igniteDamage;                                                     // �ܵ��ĵ�ȼ�˺�

    [SerializeField] private GameObject shockStrikePrefab;
    private float shockDamage;

    [SerializeField] private float ailmentDuration = 4;

    public float currentHealth;                                                     // ��ǰ����ֵ

    public System.Action onHealthChanged;                                           // 
    public bool isDead { get; private set; }

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

    /// <summary>
    /// ����buff
    /// </summary>
    /// <param name="_statToModify">�ӳ�����</param>
    /// <param name="_modifier">�ӳ���ֵ</param>
    /// <param name="_duration">buff����ʱ��</param>
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

    // ��ȡ��������
    public float GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

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

    // �ָ�����ֵ
    public virtual void IncreaseHealthBy(float _amount)
    {
        currentHealth += _amount;
        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();
        onHealthChanged?.Invoke();                                                  // Ѫ�� UI ����
    }

    // ����Ѫ��
    protected virtual void DecreaseHealthBy(float _damage)
    {
        currentHealth -= _damage;
        onHealthChanged?.Invoke();                                                  // Ѫ�� UI ����
    }

    // �ܵ��˺�
    public virtual void TakeDamage(float _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        Debug.Log(_damage);

        if (currentHealth <= 0 && !isDead)                                                     // ���Ѫ��<=0������
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

        DoMagicDamage(_targetStats);
    }

    // ��������Ŀ����ɷ���
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
        // ��ȡ����������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        float closestDistance = Mathf.Infinity;                                     // ������룬��ʼֵ�����
        Transform closestEnemy = null;                                              // ������˵�����
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                // �����⵽���ˣ���ȡ������˵�λ�ú;���
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

    // ����
    protected virtual void Die()
    {
        isDead = true;
    }
}
