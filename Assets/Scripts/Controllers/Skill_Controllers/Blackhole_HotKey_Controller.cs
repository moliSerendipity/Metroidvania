using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;                                                       // 敌人头上的热键
    private TextMeshProUGUI myText;                                                 // 显示文本的UI组件
    private Transform myEnemy;                                                      // 敌人坐标
    private Blackhole_Skill_Controller blackhole;                                   // 黑洞技能控制器脚本

    // 获取相关组件并设置热键相关参数（热键、敌人坐标、黑洞技能控制器脚本）
    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;
        myEnemy = _myEnemy.transform;
        blackhole = _myBlackhole;
        myText.text = myHotKey.ToString();                                          // 显示热键名称
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            // 当按下热键时，将敌人坐标添加到黑洞技能控制器脚本的敌人坐标列表中，并隐藏热键文本和图片
            blackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
