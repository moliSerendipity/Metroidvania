using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ������ UI ��λ��������ʾ����ͼ�ꡢ������������ʾ��Ϣ��
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;                                                                  // UI ����������
    private Image skillImage;                                                       // ����ͼ��
    [SerializeField] private string skillName;                                      // �������ƣ�������ʾ��ʾ��
    [TextArea]
    [SerializeField] private string skillDescription;                               // ����������������ʾ��ʾ��
    public bool unlocked;                                                           // ��ǰ�����Ƿ��ѽ���

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;                   // �����ü���֮ǰ���������ǰ�ü���
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;                     // �����ü���ʱ���뱣��δ�����ļ��ܣ����⼼�ܣ�
    [SerializeField] private Color lockedSkillColor;                                // δ����ʱ��ͼ����ɫ

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;                                        // ��ʼ״̬����Ϊ������ɫ
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());            // �����ťʱ���� UnlockSkill ����
    }

    // ���������߼�
    public void UnlockSkill()
    {
        // ���ǰ�ü����Ƿ�ȫ���ѽ���
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        // ��黥�⼼���Ƿ�ȫ��δ����
        for (int i = 0;i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        // ͨ����� �� ��������
        unlocked = true;
        skillImage.color = Color.white;                                             // ͼ��ָ�������ɫ
    }

    // �����ͣʱ��ʾ������ʾ
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName, skillDescription);
    }

    // ����ƿ�ʱ���ؼ�����ʾ
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
