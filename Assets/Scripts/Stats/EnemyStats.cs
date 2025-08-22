using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    // ���ˣ���ʾ����Ч��
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //enemy.DamageEffect();                                                       // ��ʾ����Ч��
    }

    // ��������������״̬
    protected override void Die()
    {
        base.Die();
        enemy.Die();                                                                // ��������״̬
    }
}
