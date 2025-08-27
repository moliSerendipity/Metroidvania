using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 抽象的物品效果（可继承扩展）
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    // 执行物品效果（由子类重写具体逻辑）
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {

    }
}
