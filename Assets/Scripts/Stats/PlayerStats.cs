using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    // ���ˣ���ʾ����Ч��
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        //player.DamageEffect();                                                      // ��ʾ����Ч��
    }

    // ��������������״̬
    protected override void Die()
    {
        base.Die();

        player.Die();                                                               // ��������״̬
        GetComponent<PlayerItemDrop>()?.GenerateDrop();                             // ������Ʒ
    }
}
