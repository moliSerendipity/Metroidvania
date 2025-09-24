using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;                             // 技能名称
    [SerializeField] private TextMeshProUGUI skillText;                             // 技能描述

    private void Start()
    {
        HideToolTip();
    }

    // 显示技能提示框
    public void ShowToolTip(string _skillName, string _skillDescription)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);                                                 // 激活提示框
    }

    // 隐藏技能提示框
    public void HideToolTip() => gameObject.SetActive(false);
}