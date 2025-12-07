using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private Enemy_DeathBringer enemy;

    private int amountOfSpells;
    private float spellCastTimer;

    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemy.amountOfSpells;
        spellCastTimer = enemy.spellCastCooldown + 0.5f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeSpellCast = Time.time;
    }

    public override void Update()
    {
        base.Update();

        spellCastTimer -= Time.deltaTime;
        if (CanSpellCast())
        {
            enemy.SpellCast();
        }
        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    private bool CanSpellCast()
    {
        if (amountOfSpells > 0 && spellCastTimer < 0)
        {
            amountOfSpells--;
            spellCastTimer = enemy.spellCastCooldown;
            return true;
        }
        return false;
    }
}
