using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrewarmManager : MonoBehaviour
{
    [System.Serializable]
    public class PrewarmItem
    {
        public GameObject prefab;                                                   // 要预热的预制体
        public int amount = 1;                                                      // 预热实例的数量
    }

    [Header("预热配置")]
    public List<PrewarmItem> prewarmList;

    private List<GameObject> tempObjects = new List<GameObject>();

    private void Start()
    {
        PrewarmAll();
    }

    // 预热所有配置的预制体
    private void PrewarmAll()
    {
        foreach (var item in prewarmList)
        {
            if (item.prefab == null || item.amount <= 0)
                continue;

            for (int i = 0; i < item.amount; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                tempObjects.Add(obj);
            }
        }

        StartCoroutine(DestroyTempObjectsNextFrame());                              // 延迟一帧清理实例
    }

    // 下一帧销毁所有临时对象
    private IEnumerator DestroyTempObjectsNextFrame()
    {
        yield return null;

        foreach (var obj in tempObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        tempObjects.Clear();
    }
}
