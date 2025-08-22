using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private EntityStats stats;
    private RectTransform myRectTransform;
    private Slider slider;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<EntityStats>();
        myRectTransform = GetComponentInParent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        entity.onFlipped += FlipUI;                                                 // ��ת�¼����FlipUI
        stats.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    // ��תUI
    private void FlipUI()
    {
        myRectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;                                                 // ȡ�������¼�
        entity.stats.onHealthChanged -= UpdateHealthUI;
    }
}
