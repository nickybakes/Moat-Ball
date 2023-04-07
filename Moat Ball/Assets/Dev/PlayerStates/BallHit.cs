using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : BasicState
{
    public BallHit()
    {
        sectionTimes = new float[] { .75f, .15f };
        canPlayerControlMove = new bool[] { false, false };
        moveSpeedMultiplier = new float[] { .3f };
        canPlayerControlRotate = new bool[] { true };
        updateMovement = new bool[] { false, true };
        alternateFriction = new bool[] { false };
        actionAvailable = new bool[][] {
            new bool[] { false, false, false, false, false },
            };

        countCooldown = new bool[][] {
            new bool[] { false, false },
            };


        nextState = PlayerState.Idle;

        stateText = "Hitting";
    }

    public override void Update(PlayerStateStats stats)
    {
        switch (stats.sectionCurrent)
        {
            case (0):
                if (stats.Status.Team == 0)
                {
                    stats.Status.aimVisual.transform.position = new Vector3(stats.Status.transform.position.x + (stats.timeInSection / sectionTimes[0]) * 28, 0, 0);
                }
                else
                {
                    stats.Status.aimVisual.transform.position = new Vector3(stats.Status.transform.position.x - (stats.timeInSection / sectionTimes[0]) * 28, 0, 0);
                }

                break;

        }
    }

    public override void OnEnterThisState(PlayerState prevState, PlayerStateStats stats)
    {
        base.OnEnterThisState(prevState, stats);
        stats.Status.aimVisual.SetActive(true);
    }

    public override void OnExitThisState(PlayerState nextState, PlayerStateStats stats)
    {
        base.OnExitThisState(nextState, stats);
        stats.Status.DisableBallHitbox();
        stats.Status.RestartCooldown(Cooldown.Attack);
        stats.Status.aimVisual.SetActive(false);
    }

    public override void SetSection(int section, int prevSection, PlayerStateStats stats)
    {
        switch (section)
        {
            case (0):
                stats.Status.Movement.SetTopDownVelocityToZero();

                break;

            case (1):
                stats.Status.ballImHitting.VolleyBall(stats.Status);
                stats.Status.ChargeAmount = 0;
                stats.Status.Movement.SetTopDownVelocityToZero();
                stats.Status.ballImHitting = null;
                break;
        }
    }
}
