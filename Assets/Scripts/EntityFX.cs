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

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    // ����һ��Э�̺���������ʵ����˸Ч��
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
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
    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
