using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;                              // ˮ��Ԥ����
    [SerializeField] private float crystalDuration;                                 // ˮ������ʱ��
    private GameObject currentCrystal;                                              // ��ǰˮ��
    Crystal_Skill_Controller currentCrystalScript;                                  // ��ǰˮ���ű�

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;                            // �Ƿ��û������ˮ��

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;                                       // �Ƿ���Ա�ը

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;                                   // �Ƿ���Գ�����λ���ƶ�
    [SerializeField] private float moveSpeed;                                       // �ƶ��ٶ�

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;                                // �Ƿ�����ڶ�ʱ����ʹ�ö��ˮ��
    [SerializeField] private int amountOfStacks;                                    // ��ʱ���ڿ���ʹ�õ����ˮ������
    [SerializeField] private float multiStackCooldown;                              // ʹ�ö��ˮ������ȴʱ��
    [SerializeField] private float UseTimeWindow;                                   // ʹ�ö��ˮ����ʱ�䴰��
    [SerializeField] private List<GameObject> crystalList = new List<GameObject>(); // ˮ���б�

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        // �����ǰû��ˮ�����������λ������һ��ˮ�������ò��������򽻻���Һ�ˮ��λ��
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            // ���ʹ�û������ˮ���������ɻ�������ˮ������������ˮ��
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
                currentCrystalScript.FinishCrystal();
        }
    }

    // �����λ������һ��ˮ�������ò���
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    // ѡ���������
    public void CurrentCrystalChooseRandomEnemy() => currentCrystalScript.ChooseRandomEnemy();

    // �Ƿ�����ڶ�ʱ����ʹ�ö��ˮ��
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalList.Count > 0)
            {
                // �����ʹ�ô����ڼ�û�����꣬��������䲢����CD
                if (crystalList.Count == amountOfStacks)
                    Invoke("ResetAbility", UseTimeWindow);

                // �����ʹ�ö��ˮ��������>0��������ˮ�������ò��������б����Ƴ�һ��ˮ��
                cooldown = 0;
                GameObject crystalToSpawn = crystalList[crystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);
                crystalList.Remove(crystalToSpawn);

                // ����Ƴ���ˮ���б�Ϊ�գ���������ȴʱ�䲢�������ˮ���б�
                if(crystalList.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    // ���ˮ���б�
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalList.Count;
        for (int i = 0; i < amountToAdd; i++)
            crystalList.Add(crystalPrefab);
    }

    // ���ö���ˮ������
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
