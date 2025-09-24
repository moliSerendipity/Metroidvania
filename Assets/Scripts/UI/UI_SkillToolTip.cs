using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;                             // ��������
    [SerializeField] private TextMeshProUGUI skillText;                             // ��������

    private void Start()
    {
        HideToolTip();
    }

    // ��ʾ������ʾ��
    public void ShowToolTip(string _skillName, string _skillDescription)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);                                                 // ������ʾ��
    }

    // ���ؼ�����ʾ��
    public void HideToolTip() => gameObject.SetActive(false);
}