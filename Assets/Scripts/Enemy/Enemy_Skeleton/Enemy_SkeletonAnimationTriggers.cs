using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    // 攻击触发动画事件
    private void AttackTrigger()
    {
        // 获取所有在攻击检测圆圈内的碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        // 遍历所有碰撞体
        foreach (Collider2D hit in colliders)
        {
            // 如果碰撞体上挂有Player脚本，让Player受伤
            if (hit.GetComponent<Player>())
            {
                enemy.stats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    // 打开反击/攻击窗口
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    // 关闭反击/攻击窗口
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
