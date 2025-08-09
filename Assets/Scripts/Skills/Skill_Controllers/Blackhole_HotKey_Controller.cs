using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;                                                       // ����ͷ�ϵ��ȼ�
    private TextMeshProUGUI myText;                                                 // ��ʾ�ı���UI���
    private Transform myEnemy;                                                      // ��������
    private Blackhole_Skill_Controller blackhole;                                   // �ڶ����ܿ������ű�

    // ��ȡ�������������ȼ���ز������ȼ����������ꡢ�ڶ����ܿ������ű���
    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;
        myEnemy = _myEnemy.transform;
        blackhole = _myBlackhole;
        myText.text = myHotKey.ToString();                                          // ��ʾ�ȼ�����
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            // �������ȼ�ʱ��������������ӵ��ڶ����ܿ������ű��ĵ��������б��У��������ȼ��ı���ͼƬ
            blackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
