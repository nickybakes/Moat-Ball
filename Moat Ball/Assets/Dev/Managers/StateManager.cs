using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Dive,
    Volley,
    Set,
    BallHit
}

public class StateManager
{
    private static BasicState[] states = {
        new Idle(),
        new Dive(),
        new Volley(),
        new Set(),
        new BallHit()
    };

    public static BasicState GetState(PlayerState state)
    {
        return states[(int) state];
    }

}
