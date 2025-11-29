using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;                                                          // 技能CD
    public float cooldownTimer;                                                     // 技能CD计时器
    protected Player player;                                                        // 玩家脚本

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;                                            // 技能CD计时器倒计时
    }

    protected virtual void CheckUnlock()
    {

    }

    // 是否可以使用技能
    public virtual bool CanUseSkill()
    {
        // 如果技能CD计时器小于0，说明技能已经冷却完毕，可以再次使用，并重置CD
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }

    // 返回最近敌人的坐标
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        // 获取附近所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25f);
        float closestDistance = Mathf.Infinity;                                     // 最近距离，初始值无穷大
        Transform closestEnemy = null;                                              // 最近敌人的坐标
        foreach(Collider2D hit in colliders)
        {
            if(hit.GetComponent<Enemy>())
            {
                // 如果检测到敌人，获取最近敌人的位置和距离
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
