using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������
public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;                             // ��������

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;                                  // �ɵ������
    [SerializeField] private float bounceGravity;                               // ���佣������
    [SerializeField] private float bounceSpeed;                                 // ����ʱ���ƶ��ٶ�

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;                                  // �ɴ�������
    [SerializeField] private float pierceGravity;                               // ���̽�������

    [Header("Spin info")]
    [SerializeField] private float maxSpinDistance = 7;                         // ��ת���ɴ����Զ����
    [SerializeField] private float spinDuration = 2;                            // ��ת����ʱ��
    [SerializeField] private float spinGravity = 1;                             // ��ת��������
    [SerializeField] private float hitCooldown = 0.35f;                         // ��ת�������

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;                            // ����Ԥ����
    [SerializeField] private Vector2 launchForce;                               // ��������
    [SerializeField] private float swordGravity;                                // ��������
    [SerializeField] private float freezeTimeDuartion;                          // �������ʱ��������
    [SerializeField] private float returnSpeed;                                 // ���չ����н����˶��ٶ�
    [SerializeField] private float maxTravelDistance;                           // ��Զ�ɴ����

    private Vector2 finalDir;                                                   // ���շ���

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;                                  // �������
    [SerializeField] private float spaceBetweenDots;                            // ��ļ��
    [SerializeField] private GameObject dotPrefab;                              // ���Ԥ����
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();                                                         // ��������׼���ߵĵ�
        SetupGravity();                                                         // ���ý�������
    }


    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        // ����ɰ�ס�Ҽ����ͼ�����λ��
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    // ���ý�������
    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    // ������
    public void CreateSword()
    {
        if (player.sword != null)
            return;
        // �ڽ�ɫ����λ��ʵ������
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        // ��ʵ�����Ľ��ϻ�ȡ���������ű�
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        // ���ý��Ĳ���
        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxSpinDistance, spinDuration, hitCooldown);
                break;
        }
        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuartion, returnSpeed, maxTravelDistance);

        // ����ʵ��������ֵ��Player�ű���GameObject���͵�sword����
        player.AssignNewSword(newSword);
        DotsActive(false);                                                      // �׳�����ʱ��ʹ���Ĺ켣�㲻�ɼ�
    }

    #region Aim
    // ��׼���򣨽�ɫλ��ָ�����λ�ã�
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;                     // ��ɫλ��
        // ���λ��
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;                     // ��ɫλ��ָ�����λ��
        return direction;
    }

    // ���õ��Ƿ񼤻�
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    // ���ɵ㲢������
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    // ������λ��
    private Vector2 DotsPosition(float _t)
    {
        // position = vt+1/2gt^2
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * _t
            + 0.5f * Physics2D.gravity * swordGravity * _t * _t;
        return position;
    }
    #endregion
}
