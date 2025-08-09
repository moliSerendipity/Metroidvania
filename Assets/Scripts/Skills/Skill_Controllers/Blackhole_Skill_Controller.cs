using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;                               // �ȼ�Ԥ����
    [SerializeField] private List<KeyCode> keyCodeList;                             // �ȼ��б�
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float maxSize;                                                          // �ڶ����ߴ�
    private float growSpeed;                                                        // �ڶ������ٶȣ���ֵ�������ԣ�
    private float shrinkSpeed;                                                      // �ڶ������ٶ�
    private float blackholeTimer;                                                   // �ڶ�����ʱ�䵹��ʱ

    private bool canGrow = true;                                                    // �ڶ��Ƿ��������
    private bool canShrink;                                                         // �ڶ��Ƿ��������
    private bool canCreateHotKeys = true;                                           // �Ƿ���Դ����ȼ�
    private bool canCloneAttack;                                                    // �Ƿ�����ͷſ�¡����
    private bool playerCanDisappear = true;                                         // ����Ƿ������ʧ

    private int amountOfAttacks = 4;                                                // �ɿ�¡��������
    private float cloneAttackCooldown = 0.4f;                                       // ��¡������ȴʱ��
    private float cloneAttackTimer;                                                 // ��¡������ʱ��

    private List<Transform> targets = new List<Transform>();                        // �ںڶ���Χ�ڵĵ��������б�
    private List<GameObject> createdHotKeys = new List<GameObject>();               // �������ȼ��б�

    public bool playerCanExitState { get; private set; }                            // ����Ƿ�����˳��ڶ�״̬

    // ���úڶ�����
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        // �������ˮ�������¡�壬����Ҳ�����ʧ
        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisappear = false;
    }

    private void Update()
    {
        blackholeTimer -= Time.deltaTime;                                           // �ڶ�����ʱ�䵹��ʱ
        // ����ʱ����������Ƿ���Ŀ��
        if (blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;                                        // ��ֹ�ظ�����ö��߼�
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
                FinishBlackholeAbility();                                           // ���޵��ˣ�ֱ�ӽ�������
        }

        // ����R��������ǰ�ͷſ�¡������������������ʱ��
        if (Input.GetKeyDown(KeyCode.R) && targets.Count > 0)
        {
            ReleaseCloneAttack();
        }

        cloneAttackTimer -= Time.deltaTime;                                         // ��¡������ȴ��ʱ��
        CloneAttackLogic();                                                         // �����¡�����߼�

        // �������������ʹ�ò�ֵƽ�����ɵ�Ŀ���С
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        // �ڶ���Сʱ��ƽ����С����ֵ������
        if (canShrink && !canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    // ���ܿ�ʼʱ���߼�
    private void ReleaseCloneAttack()
    {
        if (targets.Count < 0)
            return;

        DestroyHotKeys();                                                           // ���������ȼ���ť
        canCreateHotKeys = false;                                                   // ��ֹ�����ȼ�
        canCloneAttack = true;                                                      // ���ÿ�¡����

        if(playerCanDisappear)
        {
            PlayerManager.instance.player.MakeTransparent(true);                    // ��ұ�Ϊ͸��״̬������ڶ�Ӱ�������׶Σ�
            playerCanDisappear = false;
        }
    }

    // ��¡�����߼�
    private void CloneAttackLogic()
    {
        // ��ȴ����������ͷ��ҿɿ�¡��������>0ʱ���ӵ��������ѡ��һ��Ŀ�����Ӱ������
        if (cloneAttackTimer < 0 && canCloneAttack && amountOfAttacks > 0 && targets.Count > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            float offset = Random.Range(0, 100) > 50 ? 1 : -1;

            // ���ʹ��ˮ�������¡�壬�򴴽�ˮ����������������򴴽���¡��
            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomEnemy();
            }
            else
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(offset, 0));

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);                               // ��������ӳ�һ����β����
            }
        }
    }

    // ��ȡ�ڶ��뾶
    public float GetBlackholeRadius() => transform.localScale.x * cd.radius;

    // ���ܽ���ʱ�������߼�
    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();                                                           // �����ȼ���ť
        playerCanExitState = true;                                                  // ��ҿ����˳��ڶ�״̬
        canShrink = true;                                                           // ���úڶ�����
        canGrow = false;                                                            // ��ֹ�ڶ�����
        canCloneAttack = false;                                                     // ��ֹ��¡����
    }

    // �������д������ȼ���ť
    private void DestroyHotKeys()
    {
        if (createdHotKeys.Count <= 0)
            return;
        for (int i = 0; i < createdHotKeys.Count; i++)
        {
            Destroy(createdHotKeys[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().FreezeTime(true);                       // �������ʱ��
            CreateHotKey(collision);                                                // �����ȼ�
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>()?.FreezeTime(false);                         // �ⶳ����ʱ��
    }

    private void CreateHotKey(Collider2D collision)
    {
        // ����ȼ��б�Ϊ�ջ��ܴ����ȼ����򷵻�
        if (keyCodeList.Count <= 0 || !canCreateHotKeys)
            return;

        // ����ײ��ͷ��ʵ�����ȼ�Ԥ����,����ӵ��������ȼ��б���
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);
        // ���ѡ��һ���ȼ���ɾ��
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        // ��ȡ�ڶ��ȼ��������ű�
        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        // �����ȼ�
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    // ��ӵ������굽�б�
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
