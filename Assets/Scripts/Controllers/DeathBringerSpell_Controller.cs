using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private EntityStats myStats;

    public void SetupSpell(EntityStats _stats)
    {
        myStats = _stats;
    }

    public void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Player>())
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<EntityStats>());
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }
}
