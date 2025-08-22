using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public EntityStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;                          // ���˷���
    [SerializeField] protected float knockbackDuration;                             // ���˳���ʱ��
    protected bool isKnocked;                                                       // �Ƿ񱻻���

    [Header("Collision info")]
    public Transform attackCheck;                                                   // ������ⷶΧ
    public float attackCheckRadius;                                                 // ������ⷶΧ�뾶
    [SerializeField] protected Transform groundCheck;                               // �������
    [SerializeField] protected float groundCheckDistance;                           // ���������
    [SerializeField] protected Transform wallCheck;                                 // ǽ�����
    [SerializeField] protected float wallCheckDistance;                             // ǽ�������
    [SerializeField] protected LayerMask whatIsGround;                              // ����ͼ��

    public int facingDir { get; private set; } = 1;                                 // ����
    protected bool facingRight = true;

    public System.Action onFlipped;                                                 // ��ת�¼�


    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<EntityStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {

    }

    // �˺�Ч��
    public virtual void DamageImpact()
    {
        StartCoroutine(HitKnockback());
        Debug.Log(gameObject.name + " is damaged");
    }

    // ������Ч��
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    #region Velocity
    // �����ٶ�Ϊ0
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }

    // �����ٶ�
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)                                                            // ����������������ٶ�
            return;
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion

    #region Collision
    // ������
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // ���ǽ��
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    // ���Ƶ����ǽ�ڼ����
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDir * wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    // ��ת
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();                                                        // ��ת�¼�
    }

    // ��ת�������������������෴�ͷ�ת��
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    // ����
    public virtual void Die()
    {

    }
}
