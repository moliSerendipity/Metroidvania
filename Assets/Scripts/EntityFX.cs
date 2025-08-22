using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;                               // ��˸����ʱ��
    [SerializeField] private Material hitMat;                                   // ������ʱ���ֵĲ���
    private Material originalMat;                                               // ԭʼ����

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // �Ƿ�͸��
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    // ����һ��Э�̺���������ʵ����˸Ч��
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = originalMat;
    }

    // �����˸��������ʱ��
    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    // ȡ����˸�����ԭ������ɫ
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    private void ChillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void ShockFXFor(float _seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }
}
