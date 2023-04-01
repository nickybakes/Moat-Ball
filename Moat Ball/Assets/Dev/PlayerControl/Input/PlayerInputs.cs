using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonInput
{
    Jump,
    Volley,
    Set,
    Juke,
    Dive
}

public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;

    private bool[] buttonPressedRaw = new bool[5];

    private float[] timeSinceButtonPress = new float[5];

    private const float timeSinceButtonPressMax = .24f;

    public bool pause;

    public bool GetInputRaw(ButtonInput input, bool resetButtonIfPressed = false)
    {
        if (resetButtonIfPressed && buttonPressedRaw[(int)input])
        {
            SetInputRaw(input, false);
            return true;
        }
        return buttonPressedRaw[(int)input];
    }

    public void SetInputRaw(ButtonInput input, bool pressed)
    {
        buttonPressedRaw[(int)input] = pressed;
    }

    public bool GetInputWithBuffer(ButtonInput input, bool resetButtonIfPressed = false)
    {
        if (resetButtonIfPressed && timeSinceButtonPress[(int)input] < timeSinceButtonPressMax)
        {
            SetInputWithBuffer(input, false);
            return true;
        }
        return timeSinceButtonPress[(int)input] < timeSinceButtonPressMax;
    }

    public void SetInputWithBuffer(ButtonInput input, bool pressed)
    {
        if (pressed)
        {
            timeSinceButtonPress[(int)input] = 0;
        }
        else
        {
            timeSinceButtonPress[(int)input] = timeSinceButtonPressMax;
        }
    }

    public int jumpFrameDisable = 2;

    private void Update()
    {
        if (jumpFrameDisable < 2)
            jumpFrameDisable++;

        for (int i = 0; i < timeSinceButtonPress.Length; i++)
        {
            if (timeSinceButtonPress[i] <= timeSinceButtonPressMax)
                timeSinceButtonPress[i] += Time.deltaTime;
        }
    }


    public void OnPause(InputValue value)
    {
        PauseInput(value.isPressed);
    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDive(InputValue value)
    {
        DiveInput(value.isPressed);
    }

    public void OnJuke(InputValue value)
    {
        JukeInput(value.isPressed);
    }

    public void OnVolley(InputValue value)
    {
        VolleyInput(value.isPressed);
    }

    public void OnBlock(InputValue value)
    {
        BlockInput(value.isPressed);
    }

    public void PauseInput(bool newState)
    {
        if (newState)
        {
            // GameManager.PauseGame();
        }
    }

    public void BlockInput(bool newState)
    {
        // wasBlocking = block;
        // block = newBlockState;
    }

    public void VolleyInput(bool newState)
    {
        SetInputRaw(ButtonInput.Volley, newState);
        SetInputWithBuffer(ButtonInput.Volley, newState);
    }

    public void JukeInput(bool newState)
    {
        SetInputRaw(ButtonInput.Juke, newState);
        SetInputWithBuffer(ButtonInput.Juke, newState);
    }

    public void OnSet(InputValue value)
    {
        SetInput(value.isPressed);
    }

    public void SetInput(bool newState)
    {
        SetInputRaw(ButtonInput.Set, newState);
        SetInputWithBuffer(ButtonInput.Set, newState);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void JumpInput(bool newState)
    {
        if (newState && jumpFrameDisable >= 2)
        {
            SetInputRaw(ButtonInput.Jump, newState);
            SetInputWithBuffer(ButtonInput.Jump, newState);
        }
    }

    public void DiveInput(bool newState)
    {
        SetInputRaw(ButtonInput.Dive, newState);
        SetInputWithBuffer(ButtonInput.Dive, newState);
    }

}