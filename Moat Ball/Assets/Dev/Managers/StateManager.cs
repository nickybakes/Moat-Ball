using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle
}

public class StateManager : MonoBehaviour
{
    private static BasicState[] states = {
        


    };

    public static BasicState GetState(PlayerState state)
    {
        return states[(int) state];
    }

}
