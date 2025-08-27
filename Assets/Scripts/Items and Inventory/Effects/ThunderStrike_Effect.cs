using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 雷击效果（继承自 ItemEffect）
[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item Effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;                        // 雷击特效预制体

    // 在敌人位置生成一次雷击特效，0.6秒后销毁
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, 0.6f);
    }
}
