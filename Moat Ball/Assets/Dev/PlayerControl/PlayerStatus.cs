using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    /// <summary>
    /// starts at 0 (0 = player 1);
    /// </summary>
    private int playerNumber = -1;

    public int PlayerNumber
    {
        get => playerNumber;

        set
        {
            if (playerNumber == -1)
                playerNumber = value;
        }
    }


    private PlayerStateStats state;

    public PlayerStateStats State { get => state; }

    public PlayerMaterialHandler materialHandler;

    public PlayerHeader playerHeader;

    public float chargeAmount;
    public float chargeMax;

    private PlayerMovement movement;

    private new Transform transform;

    private PlayerInputs _input;

    public PlayerMovement Movement { get => movement; }


    void Awake()
    {
        state = new PlayerStateStats(this);

        _input = GetComponent<PlayerInputs>();


        transform = gameObject.transform;
        movement = GetComponent<PlayerMovement>();

        movement.Start();


    }

    // Update is called once per frame
    void Update()
    {
        movement.UpdateManual(state.details.updateMovement, state.details.canPlayerControlMove, state.details.canPlayerControlRotate, state.details.alternateFriction);

        state.Update();

        if (_input.GetInputRaw(ButtonInput.Dive, true) && state.ActionAvailable(PlayerAction.Dive))
        {
            Dive();
        }
    }

    public void Dive()
    {
        state.SetStateImmediate(PlayerState.Dive);
        Movement.SetTheSetForwardDirection();
        Movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
