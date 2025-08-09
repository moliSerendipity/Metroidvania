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

    // �������������¼�
    private void AttackTrigger()
    {
        // ��ȡ�����ڹ������ԲȦ�ڵ���ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        // ����������ײ��
        foreach (Collider2D hit in colliders)
        {
            // �����ײ���Ϲ���Player�ű�����Player����
            if (hit.GetComponent<Player>())
            {
                enemy.stats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    // �򿪷���/��������
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    // �رշ���/��������
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
