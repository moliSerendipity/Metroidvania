using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;                            // 克隆体的预制体
    [SerializeField] private float cloneDuration;                               // 克隆体的存在时间
    [Space]
    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;                       // 克隆体攻击力的倍数
    [SerializeField] private bool canAttack;                                    // 是否会攻击

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;                            // 是否可以创建多个克隆体
    [SerializeField] private float chanceToDuplicate;                           // 创建多个克隆体的概率

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;                                          // 是否用水晶代替克隆体

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.OnSkillUnlocked += UnlockCloneAttack;
        aggressiveCloneUnlockButton.OnSkillUnlocked += UnlockAggressiveClone;
        multipleUnlockButton.OnSkillUnlocked += UnlockMultiClone;
        crystalInsteadUnlockButton.OnSkillUnlocked += UnlockCrystalInstead;
    }

    #region Unlock region
    protected override void CheckUnlock()
    {
        UnlockCloneAttack(null);
        UnlockAggressiveClone(null);
        UnlockMultiClone(null);
        UnlockCrystalInstead(null);
    }

    private void UnlockCloneAttack(UI_SkillTreeSlot slot)
    {
        if (canAttack) return;

        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone(UI_SkillTreeSlot slot)
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone(UI_SkillTreeSlot slot)
    {
        if (canDuplicateClone) return;

        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInstead(UI_SkillTreeSlot slot)
    {
        if (crystalInsteadOfClone) return;

        if (crystalInsteadUnlockButton.unlocked)
            crystalInsteadOfClone = true;
    }
    #endregion

    // 如果能用水晶代替克隆体，则创建水晶，否则创建克隆体
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);                         // 实例化克隆体
        // 在实例化的克隆体上获取克隆体控制器脚本
        Clone_Skill_Controller newCloneScript = newClone.GetComponent<Clone_Skill_Controller>();
        // 设置克隆体参数
        newCloneScript.SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(_clonePosition), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    // 是否在反击时创建克隆体
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    // 延迟创建克隆体
    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
