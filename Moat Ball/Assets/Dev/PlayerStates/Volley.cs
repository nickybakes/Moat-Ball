using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : BasicState
{
    public Volley()
    {
        sectionTimes = new float[] { 1f, .12f, .35f };
        canPlayerControlMove = new bool[] { false };
        canPlayerControlRotate = new bool[] { true };
        updateMovement = new bool[] { true, false, false };
        alternateFriction = new bool[] { false };
        actionAvailable = new bool[][] {
            new bool[] { true, false, false, false, true },
            };

        nextState = PlayerState.Idle;

        stateText = "Volley";
    }

    public override void Update(PlayerStateStats stats)
    {
        switch (stats.sectionCurrent)
        {
            case (0):
                stats.Status.ChargeAmount += Time.deltaTime;

                break;

        }
    }

    public override void OnEnterThisState(PlayerState prevState, PlayerStateStats stats)
    {
        base.OnEnterThisState(prevState, stats);

        stats.Status.Movement.SetTopDownVelocityToZero();
    }

    public override void OnExitThisState(PlayerState nextState, PlayerStateStats stats)
    {
        base.OnExitThisState(nextState, stats);
        if (nextState != PlayerState.Dive)
            stats.Status.ChargeAmount = 0;
    }

    public override void SetSection(int section, int prevSection, PlayerStateStats stats)
    {
        switch (stats.sectionCurrent)
        {
            case (0):

                break;

            case (1):
                stats.Status.ChargeAmount = 0;

                break;
        }
    }
}
