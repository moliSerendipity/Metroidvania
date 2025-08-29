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

    // ����Ѫ��������������Ч��
    protected override void DecreaseHealthBy(float _damage)
    {
        base.DecreaseHealthBy(_damage);

        Inventory.instance.GetEquipment(EquipmentType.Armor)?.Effect(player.transform);
    }

    // ��������������״̬
    protected override void Die()
    {
        base.Die();

        player.Die();                                                               // ��������״̬
        GetComponent<PlayerItemDrop>()?.GenerateDrop();                             // ������Ʒ
    }
}
