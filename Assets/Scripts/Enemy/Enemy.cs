using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;                              // ���ͼ��

    [Header("Stunned info")]
    public float stunDuration;                                                      // ��ѣ����ʱ��
    public Vector2 stunDirection;                                                   // ��ѣ����
    protected bool canBeStunned;                                                    // �Ƿ���Ա���ѣ
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;                                                         // �ƶ��ٶ�
    public float idleTime;                                                          // ��ֹʱ��
    public float battleTime;                                                        // ����ս��״̬����ʱ��
    private float defaultMoveSpeed;                                                 // ��ʼ�ƶ��ٶ�

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;                                                    // ����CD
    [HideInInspector] public float lastTimeAttacked;                                // �ϴι�������ʱ��

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }                            // �ϴζ�������������

    protected override void Awake()
    {
        base.Awake();

        defaultMoveSpeed = moveSpeed;
        // ��ʼ��״̬��
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();                                         // ���µ�ǰ״̬
    }

    // �Ƿ񶳽���˵�ʱ�䣬�����ƶ��ٶȺͶ��������ٶȶ�Ϊ0
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    // ������˵�ʱ���������
    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    // �򿪷���/��������
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;                                                        // ���Ա�����
        counterImage.SetActive(true);                                               // ����/������ʾͼƬ�ɼ�
    }

    // �رշ���/��������
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;                                                       // ���ɱ�����
        counterImage.SetActive(false);                                              // ���ط���/������ʾͼƬ
    }
    #endregion

    // �Ƿ���Ա�����
    public virtual bool CanBeStunned()
    {
        // ������Ա����Σ���رշ���/�������ڣ�������true
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    // ���������������¼�
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    // ��¼��ǰ����������
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    // ������
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50f, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;                                                // ��������Χ���߸�Ϊ��ɫ
        // ���ƹ�����Χ����
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + facingDir * attackDistance, transform.position.y));
    }
}
