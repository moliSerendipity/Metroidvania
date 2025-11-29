using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private GameObject blackholePrefab;                            // 黑洞预制体
    [SerializeField] private float maxSize;                                         // 黑洞最大尺寸
    [SerializeField] private float growSpeed;                                       // 黑洞增长速度（插值，非线性）
    [SerializeField] private float shrinkSpeed;                                     // 黑洞收缩速度
    [SerializeField] private int amountOfAttacks = 4;                               // 可克隆攻击数量
    [SerializeField] private float cloneAttackCooldown = 0.4f;                      // 克隆攻击冷却时间
    [SerializeField] private float blackholeDuration;                               // 黑洞持续时间

    Blackhole_Skill_Controller currentBlackhole;                                    // 当前黑洞控制器脚本

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.OnSkillUnlocked += UnlockBlackHole;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackHole(null);
    }

    private void UnlockBlackHole(UI_SkillTreeSlot slot)
    {
        if (blackholeUnlocked) return;

        if (blackholeUnlockButton.unlocked)
            blackholeUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        // 在角色位置实例化黑洞
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        // 获取黑洞控制器脚本
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        // 设置黑洞参数
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    // 检查技能是否已完成
    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        // 检查玩家是否可以退出黑洞状态
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;                                                // 清除当前黑洞引用
            return true;
        }
        return false;
    }

    // 获取黑洞半径
    public float GetBlackholeRadius() => currentBlackhole ? currentBlackhole.GetBlackholeRadius() : 0;
}
