using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected float xInput;                                             // 水平输入
    protected float yInput;                                             // 垂直输入
    protected Rigidbody2D rb;
    private string animBoolName;                                        // 动画布尔值名称
    protected float stateTimer;                                         // 状态计时器
    protected bool triggerCalled;                                       // 触发器是否被调用

    public PlayerState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName)
    {
        player = __player;
        stateMachine = __stateMachine;
        animBoolName = _animBoolName;
    }

    // 进入状态时的操作
    public virtual void Enter()
    {
        rb = player.rb;                                                 // 获取玩家刚体
        player.anim.SetBool(animBoolName, true);                        // 设置动画布尔值为真，播放动画
        triggerCalled = false;                                          // 禁止触发器被调用
    }

    // 状态更新时的操作
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");    
        player.anim.SetFloat("yVelocity", rb.velocity.y);               // 设置动画垂直速度
    }

    // 退出状态时的操作
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);                       // 设置动画布尔值为假，停止动画
    }

    // 动画结束触发的事件
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;                                           // 触发器被调用
    }
}
