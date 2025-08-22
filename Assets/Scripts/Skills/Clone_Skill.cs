using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;                            // 克隆体的预制体
    [SerializeField] private float cloneDuration;                               // 克隆体的存在时间
    [Space]
    [SerializeField] private bool canAttack;                                    // 是否会攻击
    [SerializeField] private bool createCloneOnDashStart;                       // 是否在冲刺开始时创建克隆体
    [SerializeField] private bool createCloneOnDashOver;                        // 是否在冲刺结束时创建克隆体
    [SerializeField] private bool createCloneOnCounterAttack;                   // 是否在反击时创建克隆体

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;                            // 是否可以创建多个克隆体
    [SerializeField] private float chanceToDuplicate;                           // 创建多个克隆体的概率

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;                                          // 是否用水晶代替克隆体

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
        newCloneScript.SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(_clonePosition), canDuplicateClone, chanceToDuplicate, player);
    }

    // 是否在冲刺开始时创建克隆体
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    // 是否在冲刺结束时创建克隆体
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    // 是否在反击时创建克隆体
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    // 延迟创建克隆体
    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
