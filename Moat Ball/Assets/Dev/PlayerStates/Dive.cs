using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive : BasicState
{
    public Dive()
    {
        sectionTimes = new float[] { .20f, .26f };
        canPlayerControlMove = new bool[] { false };
        canPlayerControlRotate = new bool[] { false };
        updateMovement = new bool[] { true };
        alternateFriction = new bool[] { false };
        moveSpeedMultiplier = new float[] { 1.4f, .6f };
        extraFallGravityMultiplier = new float[] { .8f, 1 };
        actionAvailable = new bool[][] {
            new bool[] { false, false, true, false, false },
            };

        countCooldown = new bool[][] {
            new bool[] { false, false },
            };

        nextState = PlayerState.Idle;

        stateText = "Dive";
    }

    public override void Update(PlayerStateStats stats)
    {
        switch (stats.sectionCurrent)
        {
            case (0):
                stats.Status.Movement.SetVelocityToMoveSpeedTimesFowardDirection();

                break;

            case (1):
                stats.details.moveSpeedMultiplier = Mathf.Lerp(1.6f, .4f, stats.timeInSection / sectionTimes[1]);

                stats.Status.Movement.SetVelocityToMoveSpeedTimesFowardDirection();
                break;
        }
    }

    public override void OnExitThisState(PlayerState nextState, PlayerStateStats stats)
    {
        base.OnExitThisState(nextState, stats);
        stats.Status.RestartCooldown(Cooldown.Dive);
    }

    public override void SetSection(int section, int prevSection, PlayerStateStats stats)
    {
        stats.Status.Movement.velocity = stats.Status.Movement.velocity.normalized * stats.Status.Movement.CurrentMoveSpeed;

        if (section == 1)
        {
            stats.Status.ChargeAmount = 0;
            stats.Status.DisableBallHitbox();
        }
    }
}
