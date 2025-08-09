using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;                               // 闪烁持续时间
    [SerializeField] private Material hitMat;                                   // 被击中时表现的材质
    private Material originalMat;                                               // 原始材质

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // 定义一个协程函数，用于实现闪烁效果
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMat;
    }

    // 红白闪烁（被击中时）
    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    // 取消闪烁并变回原来的颜色
    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
