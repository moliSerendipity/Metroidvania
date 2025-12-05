using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private EntityStats myStats;

    private void Update()
    {
        if (canMove ==  true)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<EntityStats>()?.TakeDamage(damage);
            myStats.DoDamage(collision.GetComponent<EntityStats>());
            StuckInTo(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInTo(collision);
    }

    public void SetupArrow(float _speed, EntityStats _myStats)
    {
        xVelocity = _speed;
        myStats = _myStats;
    }

    private void StuckInTo(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FlipArrow()
    {
        if (flipped) return;

        xVelocity = -xVelocity;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}
