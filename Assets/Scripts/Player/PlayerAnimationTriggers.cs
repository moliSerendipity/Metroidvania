using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    // �����¼�
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

    // �������������¼�
    private void AttackTrigger()
    {
        // ��ȡ������Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        // ����������ײ��
        foreach(Collider2D hit in colliders)
        {
            // �����ײ���Ϲ���Enemy�ű�����Enemy���ˣ�������������Ч��
            if (hit.GetComponent<Enemy>())
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoMagicDamage(_target);

                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }
    }

    // ������������ʱ����IsAttackingΪfalse
    private void SetIsnotAttacking()
    {
        player.EndAttack();
    }

    // �Խ���ʱ�������
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
