using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public EntityStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7, 12);         // 击退方向
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(0.5f, 2);
    [SerializeField] protected float knockbackDuration = 0.07f;                     // 击退持续时间
    protected bool isKnocked;                                                       // 是否被击退

    [Header("Collision info")]
    public Transform attackCheck;                                                   // 攻击检测范围
    public float attackCheckRadius = 1.2f;                                          // 攻击检测范围半径
    [SerializeField] protected Transform groundCheck;                               // 地面检测点
    [SerializeField] protected float groundCheckDistance = 1;                       // 地面检测距离
    [SerializeField] protected Transform wallCheck;                                 // 墙面检测点
    [SerializeField] protected float wallCheckDistance = 0.8f;                      // 墙面检测距离
    [SerializeField] protected LayerMask whatIsGround;                              // 地面图层

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;                                 // 朝向
    protected bool facingRight = true;

    public System.Action onFlipped;                                                 // 翻转事件


    protected virtual void Awake()
    {
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

    // 伤害效果
    public virtual void DamageImpact()
    {
        StartCoroutine(HitKnockback());
        Debug.Log(gameObject.name + " is damaged");
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else
            knockbackDir = 1;
    }

    // 被击退效果
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);
        rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    public void SetupKnockbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower;

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Velocity
    // 设置速度为0
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }

    // 设置速度
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)                                                            // 如果被击退则不设置速度
            return;
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion

    #region Collision
    // 检测地面
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    // 检测墙壁
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    // 绘制地面和墙壁检测线
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDir * wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    // 翻转
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();                                                        // 翻转事件
    }

    // 翻转控制器（朝向与输入相反就翻转）
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;
        if (facingDir == -1)
            facingRight = false;
    }
    #endregion

    

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    // 死亡
    public virtual void Die()
    {

    }
}
