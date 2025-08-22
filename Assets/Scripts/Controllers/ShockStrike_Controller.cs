using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private EntityStats targetStats;
    [SerializeField] private float speed;
    private float damage;

    private Animator anim;
    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!targetStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            anim.transform.localPosition = new Vector3(0, 0.5f);

            triggered = true;
            anim.SetTrigger("Hit");
            Invoke("DamageAndSelfDestroy", 0.2f);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }

    public void Setup(float _damage, EntityStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
}
