using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;                                      // ����CD
    protected float cooldownTimer;                                                  // ����CD��ʱ��
    protected Player player;                                                        // ��ҽű�

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;                                            // ����CD��ʱ������ʱ
    }

    // �Ƿ����ʹ�ü���
    public virtual bool CanUseSkill()
    {
        // �������CD��ʱ��С��0��˵�������Ѿ���ȴ��ϣ������ٴ�ʹ�ã�������CD
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }

    // ����������˵�����
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        // ��ȡ����������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25f);
        float closestDistance = Mathf.Infinity;                                     // ������룬��ʼֵ�����
        Transform closestEnemy = null;                                              // ������˵�����
        foreach(Collider2D hit in colliders)
        {
            if(hit.GetComponent<Enemy>())
            {
                // �����⵽���ˣ���ȡ������˵�λ�ú;���
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
