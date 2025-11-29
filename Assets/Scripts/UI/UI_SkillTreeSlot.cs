using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 技能树 UI 槽位（负责显示技能图标、解锁条件和提示信息）
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;                                                                  // UI 管理器引用
    private Image skillImage;                                                       // 技能图标
    [SerializeField] private string skillName;                                      // 技能名称（用于提示显示）
    [TextArea]
    [SerializeField] private string skillDescription;                               // 技能描述（用于提示显示）
    [SerializeField] private int skillCost;                                         // 解锁技能所需金币
    public bool unlocked;                                                           // 当前技能是否已解锁
    // 事件：当这个技能槽位成功解锁时，就会触发这个事件，并把自己(UI_SkillTreeSlot)作为参数传递出去
    // 这样外部脚本就可以知道 "哪个技能被解锁了"
    public event Action<UI_SkillTreeSlot> OnSkillUnlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;                   // 解锁该技能之前必须解锁的前置技能
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;                     // 解锁该技能时必须保持未解锁的技能（互斥技能）
    [SerializeField] private Color lockedSkillColor;                                // 未解锁时的图标颜色

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkill);            // 点击按钮时调用 UnlockSkill 方法
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;                                        // 初始状态设置为锁定颜色
        if (unlocked)
            skillImage.color = Color.white;
    }

    // 解锁技能逻辑
    public void UnlockSkill()
    {
        // 金币不够就无法解锁，已解锁再点击不会扣金币
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false || unlocked == true) return;

        // 检查前置技能是否全部已解锁
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        // 检查互斥技能是否全部未解锁
        for (int i = 0;i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        // 通过检查 → 解锁技能
        unlocked = true;
        skillImage.color = Color.white;                                             // 图标恢复正常颜色
        PlayerManager.instance.currency -= skillCost;                              // 解锁后扣除金币

        // 触发事件，把当前槽位对象(this)传递给所有订阅者,这里用 Invoke(this)，所以外部可以区分“具体是哪个槽位解锁了”
        OnSkillUnlocked.Invoke(this);
    }

    // 鼠标悬停时显示技能提示
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName, skillDescription, skillCost);
    }

    // 鼠标移开时隐藏技能提示
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
            _data.skillTree[skillName] = unlocked;
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}
