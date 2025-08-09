using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;                                             // 主摄像机
    [SerializeField] private float parallaxEffect;                      // 视差效果
    private float xPosition;                                            // 当前物体的x坐标
    private float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;                               // 将当前物体的x坐标赋值给xPosition
        length = GetComponent<SpriteRenderer>().bounds.size.x;          // 获取当前物体的长度
    }

    void Update()
    {
        float distanceMove = cam.transform.position.x * (1 - parallaxEffect);
        // 计算摄像机移动的距离，乘以视差效果
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // 将当前物体的x坐标加上摄像机移动的距离，并赋值给当前物体的位置
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        // 制造一种场景无限延伸的感觉
        if (distanceMove > xPosition + length)
            xPosition += length;
        else if (distanceMove < xPosition - length)
            xPosition -= length;
    }
}
