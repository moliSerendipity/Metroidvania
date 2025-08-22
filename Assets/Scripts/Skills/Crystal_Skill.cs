using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;                              // 水晶预制体
    [SerializeField] private float crystalDuration;                                 // 水晶持续时间
    private GameObject currentCrystal;                                              // 当前水晶
    Crystal_Skill_Controller currentCrystalScript;                                  // 当前水晶脚本

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;                            // 是否用幻象代替水晶

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;                                       // 是否可以爆炸

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;                                   // 是否可以朝敌人位置移动
    [SerializeField] private float moveSpeed;                                       // 移动速度

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;                                // 是否可以在短时间内使用多个水晶
    [SerializeField] private int amountOfStacks;                                    // 短时间内可以使用的最大水晶数量
    [SerializeField] private float multiStackCooldown;                              // 使用多个水晶的冷却时间
    [SerializeField] private float UseTimeWindow;                                   // 使用多个水晶的时间窗口
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>(); // 水晶列表

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        // 如果当前没有水晶，则在玩家位置生成一个水晶并设置参数，否则交换玩家和水晶位置
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            // 如果使用幻象代替水晶，则生成幻象并销毁水晶，否则销毁水晶
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
                currentCrystalScript.FinishCrystal();
        }
    }

    // 在玩家位置生成一个水晶并设置参数
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    // 选择随机敌人
    public void CurrentCrystalChooseRandomEnemy() => currentCrystalScript.ChooseRandomEnemy();

    // 是否可以在短时间内使用多个水晶
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalList.Count > 0)
            {
                // 如果在使用窗口期间没有用完，则重新填充并进入CD
                if (crystalList.Count == amountOfStacks)
                    Invoke("ResetAbility", UseTimeWindow);

                // 如果可使用多个水晶且数量>0，则生成水晶、设置参数并从列表中移除一个水晶
                cooldown = 0;
                GameObject crystalToSpawn = crystalList[crystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);
                crystalList.Remove(crystalToSpawn);

                // 如果移除后水晶列表为空，则重置冷却时间并重新填充水晶列表
                if(crystalList.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    // 填充水晶列表
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalList.Count;
        for (int i = 0; i < amountToAdd; i++)
            crystalList.Add(crystalPrefab);
    }

    // 重置多重水晶技能
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
