using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;                            // �ڶ�Ԥ����
    [SerializeField] private float maxSize;                                         // �ڶ����ߴ�
    [SerializeField] private float growSpeed;                                       // �ڶ������ٶȣ���ֵ�������ԣ�
    [SerializeField] private float shrinkSpeed;                                     // �ڶ������ٶ�
    [SerializeField] private int amountOfAttacks = 4;                               // �ɿ�¡��������
    [SerializeField] private float cloneAttackCooldown = 0.4f;                      // ��¡������ȴʱ��
    [SerializeField] private float blackholeDuration;                               // �ڶ�����ʱ��

    Blackhole_Skill_Controller currentBlackhole;                                    // ��ǰ�ڶ��������ű�

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        // �ڽ�ɫλ��ʵ�����ڶ�
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        // ��ȡ�ڶ��������ű�
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        // ���úڶ�����
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    // ��鼼���Ƿ������
    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        // �������Ƿ�����˳��ڶ�״̬
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;                                                // �����ǰ�ڶ�����
            return true;
        }
        return false;
    }

    // ��ȡ�ڶ��뾶
    public float GetBlackholeRadius() => currentBlackhole ? currentBlackhole.GetBlackholeRadius() : 0;
}
