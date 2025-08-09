using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrewarmManager : MonoBehaviour
{
    [System.Serializable]
    public class PrewarmItem
    {
        public GameObject prefab;                                                   // ҪԤ�ȵ�Ԥ����
        public int amount = 1;                                                      // Ԥ��ʵ��������
    }

    [Header("Ԥ������")]
    public List<PrewarmItem> prewarmList;

    private List<GameObject> tempObjects = new List<GameObject>();

    private void Start()
    {
        PrewarmAll();
    }

    // Ԥ���������õ�Ԥ����
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

        StartCoroutine(DestroyTempObjectsNextFrame());                              // �ӳ�һ֡����ʵ��
    }

    // ��һ֡����������ʱ����
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
