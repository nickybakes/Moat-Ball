using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateSection
{
    Startup,
    Active,
    Recovery
}

public struct StateDetails
{
    public bool updateMovement;
    public bool canPlayerControlMove;
    public bool canPlayerControlRotate;
    public bool alternateFriction;
    public float moveSpeedMultiplier;
    public float extraFallGravityMultiplier;
    public bool[] actionAvailable;

    public StateDetails(bool updateMovement, bool canPlayerControlMove, bool canPlayerControlRotate, bool alternateFriction, float moveSpeedMultiplier, float extraFallGravityMultiplier)
    {
        this.updateMovement = updateMovement;
        this.canPlayerControlMove = canPlayerControlMove;
        this.canPlayerControlRotate = canPlayerControlRotate;
        this.alternateFriction = alternateFriction;
        this.moveSpeedMultiplier = moveSpeedMultiplier;
        this.extraFallGravityMultiplier = extraFallGravityMultiplier;
        this.actionAvailable = new bool[] { true, true, true, true, true };
    }

}

public class PlayerStateStats
{
    private PlayerStatus status;

    public StateDetails details;

    public PlayerStatus Status { get => status; }
    private PlayerState currentState;
    private BasicState currentStateClass;

    public BasicState Current { get => currentStateClass; }

    public float timeInState;
    public float timeInSection;

    public int currentSection;

    public float[] customSectionTimes;

    public PlayerStateStats(PlayerStatus status)
    {
        this.status = status;
        currentStateClass = StateManager.GetState(PlayerState.Idle);
        details = new StateDetails(true, true, true, false, 1, 1);
        SetSection(0, -1);
    }

    public void SetStateImmediate(PlayerState state)
    {
        currentStateClass.OnExitThisState(state, this);
        BasicState nextState = StateManager.GetState(state);
        nextState.OnEnterThisState(currentState, this);
        currentStateClass = nextState;
        status.playerHeader.SetStateText(currentStateClass.stateText);
        SetSection(0, -1);
        timeInSection = 0;
        timeInState = 0;
    }

    public void Update()
    {

        float[] sectionTimes = currentStateClass.useCustomSectionTimes ? customSectionTimes : currentStateClass.SectionTimes();

        if (sectionTimes[currentSection] != -1 && timeInSection > sectionTimes[currentSection])
        {
            if (currentSection == sectionTimes.Length - 1)
            {
                SetStateImmediate(currentStateClass.nextState);
            }
            else
            {
                SetSection(currentSection + 1, currentSection);
            }
        }

        currentStateClass.Update(this);
        timeInState += Time.deltaTime;
        timeInSection += Time.deltaTime;
    }

    public void SetSection(int section, int prevSection)
    {
        currentStateClass.StoreSectionDetails(section, this);
        currentStateClass.SetSection(section, prevSection, this);
        currentSection = section;
        timeInSection = 0;
    }

    public bool ActionAvailable(PlayerAction action)
    {
        return details.actionAvailable[(int) action];
    }


}
