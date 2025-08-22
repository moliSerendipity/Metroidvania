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
        entity.onFlipped += FlipUI;                                                 // 翻转事件添加FlipUI
        stats.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    // 翻转UI
    private void FlipUI()
    {
        myRectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;                                                 // 取消订阅事件
        entity.stats.onHealthChanged -= UpdateHealthUI;
    }
}
