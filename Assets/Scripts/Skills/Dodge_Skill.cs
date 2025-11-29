using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;               // Ω‚À¯…¡±‹∞¥≈•
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;                                                 //  «∑ÒΩ‚À¯…¡±‹

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.OnSkillUnlocked += UnlockDodge;
        unlockMirageDodgeButton.OnSkillUnlocked += UnlockMirageDodge;
    }

    protected override void CheckUnlock()
    {
        UnlockDodge(null);
        UnlockMirageDodge(null);
    }

    private void UnlockDodge(UI_SkillTreeSlot slot)
    {
        if (dodgeUnlocked) return;

        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge(UI_SkillTreeSlot slot)
    {
        if (unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
    }
}
