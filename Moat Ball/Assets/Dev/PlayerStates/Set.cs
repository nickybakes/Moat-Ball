using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : BasicState
{
    public Set()
    {
        sectionTimes = new float[] { .2f, .12f };
        canPlayerControlMove = new bool[] { false, false };
        moveSpeedMultiplier = new float[] { .3f };
        canPlayerControlRotate = new bool[] { true };
        updateMovement = new bool[] { false, true };
        alternateFriction = new bool[] { false };
        actionAvailable = new bool[][] {
            new bool[] { true, false, false, false, true },
            };

        countCooldown = new bool[][] {
            new bool[] { false, false },
            };

        nextState = PlayerState.Idle;

        stateText = "Set";
    }

    public override void Update(PlayerStateStats stats)
    {

    }

    public override void OnEnterThisState(PlayerState prevState, PlayerStateStats stats)
    {
        base.OnEnterThisState(prevState, stats);

    }

    public override void OnExitThisState(PlayerState nextState, PlayerStateStats stats)
    {
        base.OnExitThisState(nextState, stats);
        stats.Status.DisableBallHitbox();
        stats.Status.RestartCooldown(Cooldown.Attack);
        stats.Status.ChargeAmount = 0;
    }

    public override void SetSection(int section, int prevSection, PlayerStateStats stats)
    {
        switch (stats.sectionCurrent)
        {
            case (0):
                stats.Status.EnableSetHitbox();
                stats.Status.Movement.SetTopDownVelocityToZero();
                break;

            case (1):
                stats.Status.ChargeAmount = 0;
                stats.Status.DisableBallHitbox();
                stats.Status.Movement.SetTopDownVelocityToZero();
                break;
        }
    }
}
