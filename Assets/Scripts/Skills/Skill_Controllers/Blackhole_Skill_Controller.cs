using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;                               // 热键预制体
    [SerializeField] private List<KeyCode> keyCodeList;                             // 热键列表
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float maxSize;                                                          // 黑洞最大尺寸
    private float growSpeed;                                                        // 黑洞增长速度（插值，非线性）
    private float shrinkSpeed;                                                      // 黑洞收缩速度
    private float blackholeTimer;                                                   // 黑洞持续时间倒计时

    private bool canGrow = true;                                                    // 黑洞是否可以增长
    private bool canShrink;                                                         // 黑洞是否可以收缩
    private bool canCreateHotKeys = true;                                           // 是否可以创建热键
    private bool canCloneAttack;                                                    // 是否可以释放克隆攻击
    private bool playerCanDisappear = true;                                         // 玩家是否可以消失

    private int amountOfAttacks = 4;                                                // 可克隆攻击数量
    private float cloneAttackCooldown = 0.4f;                                       // 克隆攻击冷却时间
    private float cloneAttackTimer;                                                 // 克隆攻击计时器

    private List<Transform> targets = new List<Transform>();                        // 在黑洞范围内的敌人坐标列表
    private List<GameObject> createdHotKeys = new List<GameObject>();               // 创建的热键列表

    public bool playerCanExitState { get; private set; }                            // 玩家是否可以退出黑洞状态

    // 设置黑洞参数
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        // 如果能用水晶代替克隆体，则玩家不能消失
        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisappear = false;
    }

    private void Update()
    {
        blackholeTimer -= Time.deltaTime;                                           // 黑洞持续时间倒计时
        // 倒计时结束，检测是否有目标
        if (blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;                                        // 防止重复进入该段逻辑
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
                FinishBlackholeAbility();                                           // 若无敌人，直接结束技能
        }

        // 按下R键立即提前释放克隆攻击（用于跳过倒计时）
        if (Input.GetKeyDown(KeyCode.R) && targets.Count > 0)
        {
            ReleaseCloneAttack();
        }

        cloneAttackTimer -= Time.deltaTime;                                         // 克隆攻击冷却计时器
        CloneAttackLogic();                                                         // 处理克隆攻击逻辑

        // 如果可以增长，使用插值平滑过渡到目标大小
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        // 黑洞缩小时，平滑缩小至负值后销毁
        if (canShrink && !canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    // 技能开始时的逻辑
    private void ReleaseCloneAttack()
    {
        if (targets.Count < 0)
            return;

        DestroyHotKeys();                                                           // 销毁所有热键按钮
        canCreateHotKeys = false;                                                   // 禁止创建热键
        canCloneAttack = true;                                                      // 启用克隆攻击

        if(playerCanDisappear)
        {
            PlayerManager.instance.player.MakeTransparent(true);                    // 玩家变为透明状态（进入黑洞影分身攻击阶段）
            playerCanDisappear = false;
        }
    }

    // 克隆攻击逻辑
    private void CloneAttackLogic()
    {
        // 冷却完毕且允许释放且可克隆攻击数量>0时，从敌人中随机选择一个目标进行影分身攻击
        if (cloneAttackTimer < 0 && canCloneAttack && amountOfAttacks > 0 && targets.Count > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            float offset = Random.Range(0, 100) > 50 ? 1 : -1;

            // 如果使用水晶代替克隆体，则创建水晶并随机攻击，否则创建克隆体
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
                Invoke("FinishBlackholeAbility", 1f);                               // 攻击完后延迟一秒收尾技能
            }
        }
    }

    // 获取黑洞半径
    public float GetBlackholeRadius() => transform.localScale.x * cd.radius;

    // 技能结束时的清理逻辑
    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();                                                           // 清理热键按钮
        playerCanExitState = true;                                                  // 玩家可以退出黑洞状态
        canShrink = true;                                                           // 启用黑洞收缩
        canGrow = false;                                                            // 禁止黑洞增长
        canCloneAttack = false;                                                     // 禁止克隆攻击
    }

    // 销毁所有创建的热键按钮
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
            collision.GetComponent<Enemy>().FreezeTime(true);                       // 冻结敌人时间
            CreateHotKey(collision);                                                // 创建热键
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Enemy>()?.FreezeTime(false);                         // 解冻敌人时间
    }

    private void CreateHotKey(Collider2D collision)
    {
        // 如果热键列表为空或不能创建热键，则返回
        if (keyCodeList.Count <= 0 || !canCreateHotKeys)
            return;

        // 在碰撞体头上实例化热键预制体,并添加到创建的热键列表中
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKeys.Add(newHotKey);
        // 随机选择一个热键并删除
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        // 获取黑洞热键控制器脚本
        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
        // 设置热键
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    // 添加敌人坐标到列表
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
