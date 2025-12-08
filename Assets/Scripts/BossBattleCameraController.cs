using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CompositeCollider2D compositeCollider;
    [SerializeField] private BoxCollider2D boxCollider;
    private CinemachineConfiner camConfiner;

    [SerializeField] private BoxCollider2D leftWall;
    [SerializeField] protected BoxCollider2D rightWall;

    [Header("Boss battle")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Vector2 bossAppearPosition;
    private bool isBossDead = false;
    private GameObject newBoss;

    private void Start()
    {
        camConfiner = virtualCamera.GetComponent<CinemachineConfiner>();
        camConfiner.enabled = false;
        camConfiner.m_BoundingShape2D = compositeCollider;

        leftWall.enabled = false;
        rightWall.enabled = false;
    }

    private void Update()
    {
        if (newBoss != null)
        {
            if (newBoss.GetComponent<EnemyStats>().isDead)
            {
                camConfiner.enabled = false;
                leftWall.enabled = false;
                rightWall.enabled = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && isBossDead == false)
        {
            camConfiner.enabled = true;
            camConfiner.InvalidatePathCache();
            leftWall.enabled = true;
            rightWall.enabled = true;

            newBoss = Instantiate(bossPrefab, bossAppearPosition, Quaternion.identity);
        }
    }
}
