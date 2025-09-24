using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        
        if (statNameText != null )
            statNameText.text = statName;
    }

    void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

        // 各个属性需要分别计算最终数值，或在Stat里处理最终数值
    }
}
