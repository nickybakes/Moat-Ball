using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState
{

    /// <summary>
    /// Set the value to -1 to make the section infinite
    /// </summary>
    /// <value></value>
    private float[] sectionTimes = { -1 };

    public bool updateMovement = true;
    public bool alternateFriction = false;

    public bool canPlayerControlMove = true;

    public bool canPlayerControlRotate = true;

    public float moveSpeedMultiplier = 1;

    public float extraFallGravityMultiplier = 1;

    // public AnimationState animationState = AnimationState.Idle;

    public PlayerState nextState = PlayerState.Idle;

    public bool useCustomSectionTimes;

    public BasicState()
    {
        //if we need to set anything up for states to work, we can do it here
        //i used to put the default values for fields in here, but i moved that to above so
        //we could use base call of constructor if we need to set things up
        //such as initialize array, get references to stuff, etc
    }

    public virtual void Update(PlayerStateStats stats)
    {

    }

    /// <summary>
    /// This is called when we change TO this state
    /// </summary>
    /// <param name="nextState">the previous state we were in before this one</param>
    public virtual void OnEnterThisState(BasicState prevState)
    {

    }

    /// <summary>
    /// This is called when we change FROM this state
    /// </summary>
    /// <param name="nextState">the state to come after this one</param>
    public virtual void OnExitThisState(BasicState nextState)
    {
    }

    public virtual float[] SectionTimes(int param = 0)
    {
        return sectionTimes;
    }
}
