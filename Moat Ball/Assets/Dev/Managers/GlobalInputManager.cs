using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GlobalInputManager : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject cursorPrefab;

    public GameObject cursorPanel;

    public bool joinFromMenu;

    void Start()
    {
        // MenuManager.menu = FindObjectOfType<MenuManager>();
        // MenuManager.menu.cursors = new PlayerCursor[8];

        if(AppManager.app == null)
            return;

        ReInitializeCursors();
    }

    public void ReInitializeCursors()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                PlayerToken token = AppManager.app.playerTokens[i];
                token.input.SwitchCurrentActionMap("Cursor");

                GameObject cursor = Instantiate(cursorPrefab, cursorPanel.transform);
                token.SetUpCursorPrefab(cursor);
                // MenuManager.menu.cursors[i] = token.SetUpCursorPrefab(cursor);

                // MenuManager.menu.SolidifyCharacterDisplay(token.playerNumber);
            }
        }
    }


    public void OnPlayerJoined(PlayerInput input)
    {
        if (DoesThisInputAlreadyExist(input))
            return;

        Debug.Log(input.currentControlScheme);

        PlayerToken token = input.GetComponent<PlayerToken>();
        token.input = input;
        // token.visualPrefs = new CharacterVisualPrefs(-1);
        int playerNumber = GetLowestPlayerNumber();
        if (playerNumber != -1)
        {
            if (joinFromMenu)
            {
                input.SwitchCurrentActionMap("Cursor");
                token.playerNumber = playerNumber;
                AppManager.app.playerTokens[playerNumber - 1] = token;

                GameObject cursor = Instantiate(cursorPrefab, cursorPanel.transform);
                token.SetUpCursorPrefab(cursor);
                // MenuManager.menu.cursors[playerNumber - 1] = token.SetUpCursorPrefab(cursor);

                // audioManger.Play("controllerOn", 0.6f, 1.4f);

                // if (AppManager.app.TokenAmount == 1)
                // {
                //     MenuManager.menu.PassTitleScreen();
                //     AudioManager.aud.Play("firstJoined");
                // }

                // MenuManager.menu.SolidifyCharacterDisplay(playerNumber);
            }
            else
            {
                input.SwitchCurrentActionMap("Player");
                token.playerNumber = playerNumber;

                AppManager.app.playerTokens[playerNumber - 1] = token;

                GameObject player = Instantiate(playerPrefab);
                token.SetUpPlayerPrefab(player);
            }
        }
        else
        {
            PlayerInput.Destroy(input.gameObject);
        }
    }

    private bool DoesThisInputAlreadyExist(PlayerInput input)
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                if (AppManager.app.playerTokens[i].input == input)
                    return true;
            }
        }
        return false;
    }

    private int GetLowestPlayerNumber()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] == null)
                return i + 1;
        }
        return -1;
    }

}
