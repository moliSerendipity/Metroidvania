using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ч�����̳��� ItemEffect��
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;                           // ����Ԥ����
    [SerializeField] private Vector2 newVelocity;                                   // �����ٶ�

    // �ڵ���λ�����ɱ��𵯣��������ʼ�ٶ�
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Transform player = PlayerManager.instance.player.transform;
        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.rotation);
        newIceAndFire.GetComponent<Rigidbody2D>().velocity = newVelocity;
    }
}
