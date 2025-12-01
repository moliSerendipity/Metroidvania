using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 用于绑定按键和对应的 UI 面板
[System.Serializable]
public class UIKeyBinding
{
    public KeyCode key;                                                             // 快捷键
    public GameObject uiPanel;                                                      // 绑定的 UI 面板
}

// 游戏 UI 管理器
public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [Header("主 UI 窗口")]
    [SerializeField] private GameObject characterUI;                                // 角色属性面板
    [SerializeField] private GameObject skillTreeUI;                                // 技能树面板
    [SerializeField] private GameObject craftUI;                                    // 合成系统面板
    [SerializeField] private GameObject optionsUI;                                  // 游戏选项/设置面板
    [SerializeField] private GameObject inGameUI;                                   // 游戏内 UI（血条、技能栏等）

    [Header("子 UI 组件")]
    public UI_ItemToolTip itemToolTip;                                              // 物品提示框
    public UI_SkillToolTip skillToolTip;                                            // 技能提示框
    public UI_CraftWindow craftWindow;                                              // 合成窗口

    [Header("UI 面板快捷键绑定表")]
    public List<UIKeyBinding>  uiKeyBindings = new List<UIKeyBinding>();
    //private Dictionary<KeyCode, GameObject> uiKeyBindings;                          // 快捷键映射表

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    void Start()
    {
        SwitchTo(inGameUI);
    }

    void Update()
    {
        // 遍历所有快捷键 → 检查是否按下
        foreach (var binding in uiKeyBindings)
        {
            if (Input.GetKeyDown(binding.key))
            {
                SwitchWithKeyTo(binding.uiPanel);
                break;                                                              // 一次只处理一个输入
            }
        }
    }

    /// <summary>
    /// 切换到指定 UI，其他 UI 自动隐藏
    /// </summary>
    /// <param name="_menu">要打开的 UI </param>
    public void SwitchTo(GameObject _menu)
    {
        // 关闭所有子 UI
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!isFadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        // 打开指定 UI
        if (_menu != null)
        {
            _menu.SetActive(true);
            AudioManager.instance.PlaySFX(7);
        }

        if (GameManager.instance != null && _menu != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    /// <summary>
    /// 使用快捷键切换 UI：
    /// 如果目标 UI 已经打开 → 关闭它
    /// 如果目标 UI 关闭 → 打开它，并关闭其他 UI
    /// </summary>
    /// <param name="_menu">要打开的 UI </param>
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutinue());
    }

    IEnumerator EndScreenCoroutinue()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        if (volumeSettings.Length > 0)
        {
            for (int i = 0; i < volumeSettings.Length; i++)
            {
                if (_data.volumeSettings.TryGetValue(volumeSettings[i].parameter, out float value))
                    volumeSettings[i].LoadSlider(value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
