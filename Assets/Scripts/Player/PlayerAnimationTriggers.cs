using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // 动画事件
    private void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "AnimationTrigger":
                AnimationTrigger();
                break;
            case "AttackTrigger":
                AttackTrigger();
                break;
            case "SetIsnotAttacking":
                SetIsnotAttacking();
                break;
            case "ThrowSword":
                ThrowSword();
                break;
            default:
                break;
        }
    }

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    // 攻击触发动画事件
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2);
        // 获取攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        // 遍历所有碰撞体
        foreach(Collider2D hit in colliders)
        {
            // 如果碰撞体上挂有Enemy脚本，让Enemy受伤，并触发武器的效果
            if (hit.GetComponent<Enemy>())
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);

                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

    // 攻击动画结束时设置IsAttacking为false
    private void SetIsnotAttacking()
    {
        player.EndAttack();
    }

    // 仍剑的时候创造出剑
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
