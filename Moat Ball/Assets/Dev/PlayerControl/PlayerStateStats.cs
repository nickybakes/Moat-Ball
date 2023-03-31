using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateSection
{
    Startup,
    Active,
    Recovery
}

public class PlayerStateStats : MonoBehaviour
{
    private PlayerState currentState;
    private BasicState currentStateClass;

    public float timeInState;
    public float timeInSection;

    public int currentSection;

    public float[] customSectionTimes;

    public void SetStateImmediate(PlayerState state)
    {
        currentSection = 0;
        timeInSection = 0;
        timeInState = 0;
    }

    public void UpdateState()
    {

        float[] sectionTimes = currentStateClass.useCustomSectionTimes ? customSectionTimes : currentStateClass.SectionTimes();

        if (sectionTimes[currentSection] != -1 && timeInSection > sectionTimes[currentSection])
        {
            if (currentSection == sectionTimes.Length)
            {
                SetStateImmediate(currentStateClass.nextState);
            }
            else
            {
                SetSection(currentSection + 1);
            }
        }

        currentStateClass.Update(this);
        timeInState += Time.deltaTime;
        timeInSection += Time.deltaTime;

    }

    public void SetSection(int section)
    {
        currentSection = section;
        timeInSection = 0;
    }


}
