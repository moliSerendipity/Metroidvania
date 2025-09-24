using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 技能树 UI 槽位（负责显示技能图标、解锁条件和提示信息）
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;                                                                  // UI 管理器引用
    private Image skillImage;                                                       // 技能图标
    [SerializeField] private string skillName;                                      // 技能名称（用于提示显示）
    [TextArea]
    [SerializeField] private string skillDescription;                               // 技能描述（用于提示显示）
    public bool unlocked;                                                           // 当前技能是否已解锁

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;                   // 解锁该技能之前必须解锁的前置技能
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;                     // 解锁该技能时必须保持未解锁的技能（互斥技能）
    [SerializeField] private Color lockedSkillColor;                                // 未解锁时的图标颜色

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;                                        // 初始状态设置为锁定颜色
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());            // 点击按钮时调用 UnlockSkill 方法
    }

    // 解锁技能逻辑
    public void UnlockSkill()
    {
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
    }

    // 鼠标悬停时显示技能提示
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName, skillDescription);
    }

    // 鼠标移开时隐藏技能提示
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
