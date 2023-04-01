using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive : BasicState
{
    public Dive()
    {
        sectionTimes = new float[] { .20f, .26f };
        canPlayerControlMove = new bool[] { false, false };
        canPlayerControlRotate = new bool[] { false, false };
        updateMovement = new bool[] { true, true };
        alternateFriction = new bool[] { false, false };
        moveSpeedMultiplier = new float[] { 1.4f, .6f };
        extraFallGravityMultiplier = new float[] { .8f, 1 };
        actionAvailable = new bool[][] {
            new bool[] { false, true, true, true, false },
            new bool[] { false, true, true, true, false },

            };

        nextState = PlayerState.Idle;

        stateText = "Dive";
    }

    public override void Update(PlayerStateStats stats)
    {
        switch (stats.currentSection)
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

    public override void SetSection(int section, int prevSection, PlayerStateStats stats)
    {
        stats.Status.Movement.velocity = stats.Status.Movement.velocity.normalized * stats.Status.Movement.CurrentMoveSpeed;
    }
}
