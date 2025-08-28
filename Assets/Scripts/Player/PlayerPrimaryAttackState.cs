using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerGroundedState
{
    public int comboCounter { get; private set; }                               // 连击数
    private float lastTimeAttacked;                                             // 上次攻击结束时间
    private float comboWindow = 2f;                                             // 连击最大时间间隔
    public PlayerPrimaryAttackState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 如果连击计数器大于2或者连击间隔大于连击窗口时间，则将连击计数器重置为0
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);                   // 设置动画的连击计数器
  
        float attackDir = player.facingDir;                                     // 获取攻击方向
        if (Input.GetAxisRaw("Horizontal") != 0)                                // 如果有输入，则使用输入的方向
            attackDir = xInput;

        // 使角色在攻击时向前移动
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;                                                      // 角色在攻击时向前移动能持续的时间
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(0.15f));                           // 角色进入忙碌状态，持续0.15秒，在攻击过程中不能自由控制移动
        comboCounter++;                                                         // 连击计数器加1
        lastTimeAttacked = Time.time;                                           // 记录上次攻击时间
    }

    public override void Update()
    {
        base.Update();

        // 如果攻击时向前移动的时间超过stateTimer，则禁止角色移动
        if (stateMachine.currentState == this && stateTimer < 0)
            player.SetZeroVelocity();

        // 如果触发了状态改变，即攻击动画播放完，则切换到空闲状态
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
