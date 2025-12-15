using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 剑的种类
public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;                             // 剑的种类

    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;                                  // 可弹射次数
    [SerializeField] private float bounceGravity;                               // 弹射剑的重力
    [SerializeField] private float bounceSpeed;                                 // 弹射时的移动速度

    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;                                  // 可穿刺数量
    [SerializeField] private float pierceGravity;                               // 穿刺剑的重力

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float maxSpinDistance = 7;                         // 旋转剑可达的最远距离
    [SerializeField] private float spinDuration = 2;                            // 旋转持续时间
    [SerializeField] private float spinGravity = 1;                             // 旋转剑的重力
    [SerializeField] private float hitCooldown = 0.35f;                         // 旋转攻击间隔

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }                             // 是否解锁剑技能
    [SerializeField] private GameObject swordPrefab;                            // 剑的预制体
    [SerializeField] private Vector2 launchForce;                               // 发射力度
    [SerializeField] private float swordGravity;                                // 剑的重力
    [SerializeField] private float freezeTimeDuartion;                          // 冻结敌人时间持续多久
    [SerializeField] private float returnSpeed;                                 // 回收过程中剑的运动速度
    [SerializeField] private float maxTravelDistance;                           // 最远可达距离

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    private Vector2 finalDir;                                                   // 最终方向

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;                                  // 点的数量
    [SerializeField] private float spaceBetweenDots;                            // 点的间隔
    [SerializeField] private GameObject dotPrefab;                              // 点的预制体
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();                                                         // 先生成瞄准曲线的点
        SetupGravity();                                                         // 设置剑的重力

        swordUnlockButton.OnSkillUnlocked += UnlockSword;
        bounceUnlockButton.OnSkillUnlocked += UnlockBounceSword;
        pierceUnlockButton.OnSkillUnlocked += UnlockPierceSword;
        spinUnlockButton.OnSkillUnlocked += UnlockSpinSword;
        timeStopUnlockButton.OnSkillUnlocked += UnlockTimeStop;
        vulnerableUnlockButton.OnSkillUnlocked += UnlockVulnerable;
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        // 如果松按住右键，就计算点的位置
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    // 设置剑的重力
    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    // 创建剑
    public void CreateSword()
    {
        if (player.sword != null)
            return;
        // 在角色所处位置实例化剑
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // 在实例化的剑上获取剑控制器脚本
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        // 设置剑的参数
        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxSpinDistance, spinDuration, hitCooldown);
                break;
        }
        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuartion, returnSpeed, maxTravelDistance);

        // 将该实例化剑赋值给Player脚本的GameObject类型的sword变量
        player.AssignNewSword(newSword);
        DotsActive(false);                                                      // 抛出剑的时候使剑的轨迹点不可见
    }

    #region Unlock region
    protected override void CheckUnlock()
    {
        UnlockSword(null);
        UnlockBounceSword(null);
        UnlockSpinSword(null);
        UnlockPierceSword(null);
        UnlockTimeStop(null);
        UnlockVulnerable(null);
    }

    private void UnlockSword(UI_SkillTreeSlot slot)
    {
        if (swordUnlocked) return;

        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword(UI_SkillTreeSlot slot)
    {
        if (swordType == SwordType.Bounce) return;

        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    private void UnlockSpinSword(UI_SkillTreeSlot slot)
    {
        if (swordType == SwordType.Spin) return;

        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    private void UnlockPierceSword(UI_SkillTreeSlot slot)
    {
        if (swordType == SwordType.Pierce) return;

        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }

    private void UnlockTimeStop(UI_SkillTreeSlot slot)
    {
        if (timeStopUnlocked) return;

        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVulnerable(UI_SkillTreeSlot slot)
    {
        if (vulnerableUnlocked) return;

        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }
    #endregion

    #region Aim region
    // 瞄准方向（角色位置指向鼠标位置）
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;                     // 角色位置
        // 鼠标位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;                     // 角色位置指向鼠标位置
        return direction;
    }

    // 设置点是否激活
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    // 生成点并不激活
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    // 计算点的位置
    private Vector2 DotsPosition(float _t)
    {
        // position = vt+1/2gt^2
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * _t
            + 0.5f * Physics2D.gravity * swordGravity * _t * _t;
        return position;
    }
    #endregion
}
