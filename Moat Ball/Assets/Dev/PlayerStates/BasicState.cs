using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    Jump,
    Volley,
    Set,
    Juke,
    Dive
}

/// <summary>
///ACTIONS: Jump, Volley, Set, Juke, Dive
///COOLDOWNS: attack, dive
/// </summary>
public class BasicState
{

    /// <summary>
    /// Set the value to -1 to make the section infinite
    /// </summary>
    /// <value></value>
    protected float[] sectionTimes = { -1 };

    public string stateText = "";

    protected bool[] updateMovement = { true };

    protected bool[] alternateFriction = { false };

    protected bool[] canPlayerControlMove = { true };

    protected bool[] canPlayerControlRotate = { true };

    protected float[] moveSpeedMultiplier = { 1 };

    protected float[] extraFallGravityMultiplier = { 1 };
    protected bool[][] actionAvailable = { new bool[] { true, true, true, true, true } };
    protected bool[][] countCooldown = { new bool[] { true, true } };



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
    public virtual void OnEnterThisState(PlayerState prevState, PlayerStateStats stats)
    {

    }

    /// <summary>
    /// This is called when we change FROM this state
    /// </summary>
    /// <param name="nextState">the state to come after this one</param>
    public virtual void OnExitThisState(PlayerState nextState, PlayerStateStats stats)
    {
    }

    public void StoreSectionDetails(int section, PlayerStateStats stats)
    {
        stats.details.updateMovement = updateMovement[Mathf.Min(section, updateMovement.Length - 1)];
        stats.details.canPlayerControlMove = canPlayerControlMove[Mathf.Min(section, canPlayerControlMove.Length - 1)];
        stats.details.canPlayerControlRotate = canPlayerControlRotate[Mathf.Min(section, canPlayerControlRotate.Length - 1)];
        stats.details.alternateFriction = alternateFriction[Mathf.Min(section, alternateFriction.Length - 1)];
        stats.details.moveSpeedMultiplier = moveSpeedMultiplier[Mathf.Min(section, moveSpeedMultiplier.Length - 1)];
        stats.details.extraFallGravityMultiplier = extraFallGravityMultiplier[Mathf.Min(section, extraFallGravityMultiplier.Length - 1)];

        for (int i = 0; i < stats.details.actionAvailable.Length; i++)
        {
            stats.details.actionAvailable[i] = actionAvailable[Mathf.Min(section, actionAvailable.Length - 1)][i];
        }

        for (int i = 0; i < stats.details.countCooldown.Length; i++)
        {
            stats.details.countCooldown[i] = countCooldown[Mathf.Min(section, countCooldown.Length - 1)][i];
        }
    }

    public virtual void SetSection(int section, int prevSection, PlayerStateStats stats)
    {

    }

    public virtual float[] SectionTimes(int param = 0)
    {
        return sectionTimes;
    }
}
