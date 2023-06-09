using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum Scenes
{
    MENU_InitApp_01 = 0,
    MENU_MainMenu_01 = 1,
    MAP_Arena_01 = 2
}

public struct GameSettings
{
    // Basic Settings
    public int cameraShake;
    public int numberOfFloorColumns;
    public int oneSideWidthAtStart;
    public int numberOfBalls;
    public int allowedBounces;

    public GameSettings(
        int cameraShake,
        int numberOfFloorColumns,
        int oneSideWidthAtStart,
        int numberOfBalls,
        int numAllowedBounces
        )
    {
        this.cameraShake = cameraShake;
        this.numberOfFloorColumns = numberOfFloorColumns;
        this.oneSideWidthAtStart = oneSideWidthAtStart;
        this.numberOfBalls = numberOfBalls;
        this.allowedBounces = numAllowedBounces;
    }
}

public struct AudioSettings
{
    public int master;
    public int music;
    public int announcer;
    public int sfx;
    public int ambience;

    public AudioSettings(int master, int music, int announcer, int sfx, int ambience)
    {
        this.master = master;
        this.music = music;
        this.announcer = announcer;
        this.sfx = sfx;
        this.ambience = ambience;
    }
}


public class AppManager : MonoBehaviour
{

    public static AppManager app;

    public GameSettings gameSettings;
    public AudioSettings audioSettings;

    private Scenes currentScene;
    private Scenes previousScene;

    [HideInInspector]
    public bool ableToRemovePlayer;

    [HideInInspector]
    /// <summary>
    /// Player number of the current champion. -1 if no champion yet!
    /// </summary>
    public int currentChampion = -1;

    public Scenes CurrentScene
    {
        get { return currentScene; }
    }

    public Scenes PreviousScene
    {
        get { return previousScene; }
    }

    public int TokenAmount
    {
        get
        {
            int amount = 0;
            for (int i = 0; i < playerTokens.Length; i++)
            {
                if (playerTokens[i])
                    amount++;
            }
            return amount;
        }
    }

    public int RequestedPlayerAmount
    {
        get
        {
            return Mathf.Min(8, TokenAmount);
        }
    }

    public PlayerToken[] playerTokens;

    // Start is called before the first frame update
    void Start()
    {
        app = this;
        currentScene = Scenes.MENU_InitApp_01;
        DontDestroyOnLoad(gameObject);

        playerTokens = new PlayerToken[8];

        gameSettings = new GameSettings(5, 4, 11, 1, 1);

        audioSettings = new AudioSettings(10, 10, 10, 10, 10);


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex((int)Scenes.MENU_InitApp_01))
            SwitchToScene(Scenes.MENU_MainMenu_01);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemovePlayerToken(PlayerToken token)
    {
        if (!ableToRemovePlayer)
            return;

        // if (AudioManager.aud)
        // {
        //     AudioManager.aud.Play("controllerOff");
        // }

        for (int i = 0; i < playerTokens.Length; i++)
        {
            if (playerTokens[i] == token)
            {
                PlayerInput.Destroy(playerTokens[i].gameObject);
                playerTokens[i] = null;
                return;
            }
        }
    }

    public void RemovePlayerToken(int playerNumber)
    {
        if (!ableToRemovePlayer)
            return;

        RemovePlayerToken(playerTokens[playerNumber]);
        // if (TokenAmount == 0)
        // {
        //     MenuManager.menu.ReturnToTitleScreen();
        // }
    }

    public void ClearPlayerTokens()
    {
        for (int i = 0; i < playerTokens.Length; i++)
        {
            if (playerTokens[i] != null)
                PlayerInput.Destroy(playerTokens[i].gameObject);
        }
        playerTokens = new PlayerToken[8];
    }

    public void SwitchToScene(Scenes s)
    {
        if (currentScene == Scenes.MENU_MainMenu_01 && s == Scenes.MENU_MainMenu_01)
        {
            ClearPlayerTokens();
        }

        StartCoroutine(StartLoadProcess(s));

        previousScene = currentScene;
        currentScene = s;
    }

    private IEnumerator StartLoadProcess(Scenes s)
    {
        float time = 1f;
        if (currentScene == Scenes.MENU_InitApp_01)
            time = 0;
        yield return new WaitForSecondsRealtime(time);
        LoadScene(s);
    }

    private void LoadScene(Scenes s)
    {
        CheckIfLoadingDone(SceneManager.LoadSceneAsync((int)s, LoadSceneMode.Single));
    }

    private IEnumerator CheckIfLoadingDone(AsyncOperation operation)
    {
        if (!operation.isDone)
        {
            yield return null;
        }
    }
}
