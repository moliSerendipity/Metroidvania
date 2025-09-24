using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ڰ󶨰����Ͷ�Ӧ�� UI ���
[System.Serializable]
public class UIKeyBinding
{
    public KeyCode key;                                                             // ��ݼ�
    public GameObject uiPanel;                                                      // �󶨵� UI ���
}

// ��Ϸ UI ������
public class UI : MonoBehaviour
{
    [Header("�� UI ����")]
    [SerializeField] private GameObject characterUI;                                // ��ɫ�������
    [SerializeField] private GameObject skillTreeUI;                                // ���������
    [SerializeField] private GameObject craftUI;                                    // �ϳ�ϵͳ���
    [SerializeField] private GameObject optionsUI;                                  // ��Ϸѡ��/�������

    [Header("�� UI ���")]
    public UI_ItemToolTip itemToolTip;                                              // ��Ʒ��ʾ��
    public UI_SkillToolTip skillToolTip;                                            // ������ʾ��
    public UI_CraftWindow craftWindow;                                              // �ϳɴ���

    [Header("UI ����ݼ��󶨱�")]
    public List<UIKeyBinding>  uiKeyBindings = new List<UIKeyBinding>();
    //private Dictionary<KeyCode, GameObject> uiKeyBindings;                          // ��ݼ�ӳ���

    void Start()
    {
        SwitchTo(null);                                                             // ��ʼ״̬���ر����� UI
    }

    void Update()
    {
        // �������п�ݼ� �� ����Ƿ���
        foreach (var binding in uiKeyBindings)
        {
            if (Input.GetKeyDown(binding.key))
            {
                SwitchWithKeyTo(binding.uiPanel);
                break;                                                              // һ��ֻ����һ������
            }
        }
    }

    /// <summary>
    /// �л���ָ�� UI������ UI �Զ�����
    /// </summary>
    /// <param name="_menu">Ҫ�򿪵� UI </param>
    public void SwitchTo(GameObject _menu)
    {
        // �ر������� UI
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        // ��ָ�� UI
        if (_menu != null)
            _menu.SetActive(true);
    }

    /// <summary>
    /// ʹ�ÿ�ݼ��л� UI��
    /// ���Ŀ�� UI �Ѿ��� �� �ر���
    /// ���Ŀ�� UI �ر� �� ���������ر����� UI
    /// </summary>
    /// <param name="_menu">Ҫ�򿪵� UI </param>
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
