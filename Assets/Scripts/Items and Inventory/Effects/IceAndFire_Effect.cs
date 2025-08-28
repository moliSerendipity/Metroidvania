using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ч�����̳��� ItemEffect��
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;                           // ����Ԥ����
    [SerializeField] private Vector2 newVelocity;                                   // �����ٶ�

    // �����ι���ʱ���ڵ���λ�����ɱ��𵯣��������ʼ�ٶȣ�����5�������
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;
        // �Ƿ��ǵ����ι���
        bool thirdAttack = player.primaryAttack.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = newVelocity * player.facingDir;
            Destroy(newIceAndFire, 5);
        }
    }
}
