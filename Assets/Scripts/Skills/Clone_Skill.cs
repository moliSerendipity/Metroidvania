using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;                            // ��¡���Ԥ����
    [SerializeField] private float cloneDuration;                               // ��¡��Ĵ���ʱ��
    [Space]
    [SerializeField] private bool canAttack;                                    // �Ƿ�ṥ��
    [SerializeField] private bool createCloneOnDashStart;                       // �Ƿ��ڳ�̿�ʼʱ������¡��
    [SerializeField] private bool createCloneOnDashOver;                        // �Ƿ��ڳ�̽���ʱ������¡��
    [SerializeField] private bool createCloneOnCounterAttack;                   // �Ƿ��ڷ���ʱ������¡��

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;                            // �Ƿ���Դ��������¡��
    [SerializeField] private float chanceToDuplicate;                           // ���������¡��ĸ���

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;                                          // �Ƿ���ˮ�������¡��

    // �������ˮ�������¡�壬�򴴽�ˮ�������򴴽���¡��
    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);                         // ʵ������¡��
        // ��ʵ�����Ŀ�¡���ϻ�ȡ��¡��������ű�
        Clone_Skill_Controller newCloneScript = newClone.GetComponent<Clone_Skill_Controller>();
        // ���ÿ�¡�����
        newCloneScript.SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(_clonePosition), canDuplicateClone, chanceToDuplicate, player);
    }

    // �Ƿ��ڳ�̿�ʼʱ������¡��
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    // �Ƿ��ڳ�̽���ʱ������¡��
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    // �Ƿ��ڷ���ʱ������¡��
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    // �ӳٴ�����¡��
    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
