using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;                                             // �������
    [SerializeField] private float parallaxEffect;                      // �Ӳ�Ч��
    private float xPosition;                                            // ��ǰ�����x����
    private float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;                               // ����ǰ�����x���긳ֵ��xPosition
        length = GetComponent<SpriteRenderer>().bounds.size.x;          // ��ȡ��ǰ����ĳ���
    }

    void Update()
    {
        float distanceMove = cam.transform.position.x * (1 - parallaxEffect);
        // ����������ƶ��ľ��룬�����Ӳ�Ч��
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // ����ǰ�����x�������������ƶ��ľ��룬����ֵ����ǰ�����λ��
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        // ����һ�ֳ�����������ĸо�
        if (distanceMove > xPosition + length)
            xPosition += length;
        else if (distanceMove < xPosition - length)
            xPosition -= length;
    }
}
