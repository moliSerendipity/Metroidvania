using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �׻�Ч�����̳��� ItemEffect��
[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item Effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;                        // �׻���ЧԤ����

    // �ڵ���λ������һ���׻���Ч��0.6�������
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, 0.6f);
    }
}
