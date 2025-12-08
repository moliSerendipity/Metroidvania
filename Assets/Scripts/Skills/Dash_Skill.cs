using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;                     // Dash 技能对应的 UI 槽位
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;              // “冲刺时生成分身”技能槽位
    public bool cloneOnDashUnlocked { get; private set; }
    
    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;           // “冲刺到达终点时生成分身”技能槽位
    public bool cloneOnArrivalUnlocked { get; private set; }
    
    protected override void Start()
    {
        base.Start();

        // 当 dashUnlockButton 解锁时，触发事件，把 dashUnlocked 设为 true
        dashUnlockButton.OnSkillUnlocked += UnlockDash;
        // 当 cloneOnDashUnlockButton 解锁时，触发事件，把 cloneOnDashUnlocked 设为 true
        cloneOnDashUnlockButton.OnSkillUnlocked += UnlockCloneOnDash;
        // 当 cloneOnArrivalUnlockButton 解锁时，触发事件，把 cloneOnArrivalUnlocked 设为 true
        cloneOnArrivalUnlockButton.OnSkillUnlocked += UnlockCloneOnArrival;
    }

    protected override void CheckUnlock()
    {
        UnlockDash(null);
        UnlockCloneOnDash(null);
        UnlockCloneOnArrival(null);
    }

    private void UnlockDash(UI_SkillTreeSlot slot)
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash(UI_SkillTreeSlot slot)
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival(UI_SkillTreeSlot slot)
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    // 是否在冲刺开始时创建克隆体
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    // 是否在冲刺结束时创建克隆体
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }


    public override void UseSkill()
    {
        base.UseSkill();
    }
}
