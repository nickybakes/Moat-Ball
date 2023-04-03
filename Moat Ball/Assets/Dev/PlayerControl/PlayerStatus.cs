using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cooldown
{
    Attack,
    Dive,
}

public class PlayerStatus : MonoBehaviour
{

    /// <summary>
    /// starts at 0 (0 = player 1);
    /// </summary>
    private int playerNumber = -1;

    /// <summary>
    /// starts at 0 (0 = player 1);
    /// </summary>
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

    [HideInInspector]
    public PlayerHeader header;

    [HideInInspector]
    private Timer[] cooldowns;

    public Hitbox hitbox;

    private float chargeAmount;

    public float ChargeAmount
    {
        get => chargeAmount;
        set
        {
            chargeAmount = Mathf.Clamp(value, 0, chargeMax);
            header.UpdateChargeMeter();
        }
    }

    public const float chargeMax = 1;

    private PlayerMovement movement;

    private new Transform transform;

    private PlayerInputs _input;

    public PlayerMovement Movement { get => movement; }


    void Awake()
    {
        state = new PlayerStateStats(this);

        hitbox.Status = this;

        cooldowns = new Timer[] {
            new Timer(.35f),
            new Timer(.35f),
        };

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

        for (int i = 0; i < cooldowns.Length; i++)
        {
            if (state.details.countCooldown[i])
                cooldowns[i].Update();
        }

        if (_input.GetInputRaw(ButtonInput.Volley) && state.ActionAvailable(PlayerAction.Volley) && CooldownDone(Cooldown.Attack))
        {
            StartVolleyCharge();
        }

        if (state.Current == PlayerState.Volley && state.sectionCurrent == 0 && !_input.GetInputRaw(ButtonInput.Volley))
        {
            InputEndVolleyCharge();
        }

        if (_input.GetInputRaw(ButtonInput.Dive, true) && state.ActionAvailable(PlayerAction.Dive) && CooldownDone(Cooldown.Dive))
        {
            Dive();
        }
    }

    public void DetectBallInHitbox(Ball ball)
    {
        if (state.Current == PlayerState.Volley)
        {
            Debug.Log("Volley");

        }
    }

    public void EnableVolleyHitbox()
    {
        hitbox.gameObject.SetActive(true);
    }

    public void DisableVolleyHitbox()
    {
        hitbox.gameObject.SetActive(false);
    }

    public void StartVolleyCharge()
    {
        state.SetStateImmediate(PlayerState.Volley);
    }

    public void InputEndVolleyCharge()
    {
        state.SetSection(1, 0);
    }

    public void Dive()
    {
        state.SetStateImmediate(PlayerState.Dive);
        Movement.SetTheSetForwardDirection();
        Movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public bool CooldownDone(Cooldown cooldown)
    {
        return cooldowns[(int)cooldown].Done();
    }

    public void RestartCooldown(Cooldown cooldown)
    {
        cooldowns[(int)cooldown].Restart();
    }
}
