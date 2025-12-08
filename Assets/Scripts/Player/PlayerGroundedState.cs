using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player __player, PlayerStateMachine __stateMachine, string _animBoolName) : base(__player, __stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // 当按下R键时，将状态机切换到player.blackhole状态
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        {
            if (player.skill.blackhole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown");
                return;
            }
            stateMachine.ChangeState(player.blackhole);
        }

        // 当按下鼠标右键且未丢出剑时，将状态机切换到player.aimSword状态，HasNoSword()可以回收剑
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked && SkillManager.instance.sword.CanUseSkill())
            stateMachine.ChangeState(player.aimSword);

        // 当按下Q键时，将状态机切换到player.counterAttack状态
        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked && SkillManager.instance.parry.CanUseSkill())
            stateMachine.ChangeState(player.counterAttack);

        // 当按下鼠标左键时，将状态机切换到player.primaryAttack状态
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.IsAttacking)
        {
            stateMachine.ChangeState(player.primaryAttack);
            player.StartAttack();                                                   // 设置IsAttacking为true
            return;
        }

        // 当玩家不在地面上时，将状态机切换到player.airState状态
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        // 当按下空格键且玩家在地面上时，将状态机切换到player.jumpState状态
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    // 是否没有丢出剑，如果丢出剑就要回收剑
    private bool HasNoSword()
    {
        if (!player.sword)
            return true;
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
