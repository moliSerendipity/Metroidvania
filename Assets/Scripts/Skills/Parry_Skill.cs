using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.OnSkillUnlocked += UnlockParry;
        restoreUnlockButton.OnSkillUnlocked += UnlockParryRestore;
        parryWithMirageUnlockButton.OnSkillUnlocked += UnlockParryWithMirage;
    }

    protected override void CheckUnlock()
    {
        UnlockParry(null);
        UnlockParryRestore(null);
        UnlockParryWithMirage(null);
    }

    private void UnlockParry(UI_SkillTreeSlot slot)
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore(UI_SkillTreeSlot slot)
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockParryWithMirage(UI_SkillTreeSlot slot)
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            float restoreAmount = player.stats.GetMaxHealthValue() * restoreHealthPercentage;
            player.stats.IncreaseHealthBy(restoreAmount);
            Debug.Log(restoreAmount);
        }
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
