using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager game;

    [SerializeField]
    private TempSceneSwap tempSceneSwap;

    public GameObject playerPrefab;
    public GameObject floorColumnPrefab;


    [HideInInspector]
    public List<PlayerStatus> allPlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> alivePlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> eliminatedPlayerStatuses;

    public bool dontUpdateGameplay;

    private float columnWidth;

    private float moatCenterX;

    private float leftTeamCenterX;

    private float rightTeamCenterX;

    private List<Animator> leftTeamColumns;
    private List<Animator> rightTeamColumns;


    // Start is called before the first frame update
    void Start()
    {
        if (AppManager.app == null)
        {
            tempSceneSwap.gameObject.SetActive(true);
            tempSceneSwap.InitApp();
            return;
        }

        GameManager.game = this;
        Physics.autoSimulation = true;

        allPlayerStatuses = new List<PlayerStatus>();
        alivePlayerStatuses = new List<PlayerStatus>();
        eliminatedPlayerStatuses = new List<PlayerStatus>();

        leftTeamColumns = new List<Animator>();
        rightTeamColumns = new List<Animator>();

        // InitializeCursors();
        // hudManager.cursorPanel.gameObject.SetActive(false);

        // if (AppManager.app.RequestedPlayerAmount == 1)
        //     playersInvincible = true;

        SpawnFloorColumns();
        SpawnPlayerPrefabsSimple();
        // MoveAllPlayersToGround();
        // SpawnRing();

        // StartCoroutine(IntroSequence());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalculateCenterPoints()
    {
        leftTeamCenterX = -.5f * (AppManager.app.gameSettings.oneSideWidthAtStart - (leftTeamColumns[0].transform.position.x + columnWidth));
        rightTeamCenterX = .5f * (AppManager.app.gameSettings.oneSideWidthAtStart + (rightTeamColumns[0].transform.position.x - columnWidth));
        moatCenterX = .5f * ((rightTeamColumns[0].transform.position.x - columnWidth) + (leftTeamColumns[0].transform.position.x + columnWidth));
        Debug.Log(leftTeamCenterX + ", " + rightTeamCenterX + ", " + moatCenterX);
    }

    public void SpawnFloorColumns()
    {
        columnWidth = ((float)AppManager.app.gameSettings.oneSideWidthAtStart / (float)AppManager.app.gameSettings.numberOfFloorColumns);

        Debug.Log(columnWidth);

        for (int i = 0; i < AppManager.app.gameSettings.numberOfFloorColumns; i++)
        {
            int flip = -1;
            while (flip <= 1)
            {
                Debug.Log(flip * (2 + (columnWidth / 2.0f) * i));
                GameObject columnObject = GameObject.Instantiate(floorColumnPrefab, new Vector3(flip * ((3 + columnWidth/2.0f) + (columnWidth * i)), 0, 0), Quaternion.identity);
                columnObject.transform.localScale = new Vector3(columnWidth, 1, 1);
                if (flip == -1)
                    leftTeamColumns.Add(columnObject.GetComponent<Animator>());
                else
                    rightTeamColumns.Add(columnObject.GetComponent<Animator>());

                flip += 2;
            }
        }

        CalculateCenterPoints();
    }

    public void SpawnPlayerPrefabsSimple()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                bool leftSide = i % 2 == 0;
                PlayerToken token = AppManager.app.playerTokens[i];
                token.input.SwitchCurrentActionMap("Player");

                GameObject player = Instantiate(playerPrefab, new Vector3(leftSide ? leftTeamCenterX : rightTeamCenterX, 3, 0), Quaternion.identity);
                PlayerStatus status = token.SetUpPlayerPrefab(player);

                allPlayerStatuses.Add(status);
                alivePlayerStatuses.Add(status);

                // hudManager.CreatePlayerHeader(status);
            }
        }
    }


    public void SpawnPlayerPrefabs()
    {
        // Transform spawnPoints = gameSceneSettings.transform.GetChild(Mathf.Max(0, AppManager.app.RequestedPlayerAmount - 2));
        // spawnPoints.gameObject.SetActive(true);
        // int[] spawnPointOrder = new int[spawnPoints.childCount];

        // for (int i = 0; i < spawnPointOrder.Length; i++)
        // {
        //     spawnPointOrder[i] = i;
        // }

        // ShuffleArray(spawnPointOrder);

        // int j = 0;

        // int botsAdded = 0;

        // int botAmountToAdd = AppManager.app.gameSettings.botsOnly ? Mathf.Max(AppManager.app.gameSettings.botAmount, 1) : AppManager.app.gameSettings.botAmount;

        // botVisualPrefs = new CharacterVisualPrefs[AppManager.app.RequestedPlayerAmount];

        // for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        // {
        //     if (AppManager.app.playerTokens[i] != null && !AppManager.app.gameSettings.botsOnly)
        //     {
        //         PlayerToken token = AppManager.app.playerTokens[i];
        //         token.input.SwitchCurrentActionMap("Player");

        //         GameObject player = Instantiate(playerPrefab, spawnPoints.GetChild(spawnPointOrder[j]).position, spawnPoints.GetChild(spawnPointOrder[j]).rotation);
        //         PlayerStatus status = token.SetUpPlayerPrefab(player);
        //         j++;

        //         allPlayerStatuses.Add(status);
        //         alivePlayerStatuses.Add(status);

        //         hudManager.CreatePlayerHeader(status);
        //     }
        //     else if (botsAdded < botAmountToAdd)
        //     {
        //         //spawn bot to fill in the player slots
        //         CharacterVisualPrefs visualPrefs = new CharacterVisualPrefs(Random.Range(0, 16), Random.Range(0, 7), Random.Range(0, 16));

        //         GameObject player = Instantiate(playerPrefab, spawnPoints.GetChild(spawnPointOrder[j]).position, spawnPoints.GetChild(spawnPointOrder[j]).rotation);
        //         PlayerStatus status = PlayerToken.SetUpBotPlayerPrefab(player, i + 1, visualPrefs);
        //         j++;

        //         botVisualPrefs[i] = visualPrefs;

        //         GameObject bot = Instantiate(botControllerPrefab);

        //         BotController botController = bot.GetComponent<BotController>();
        //         botController.Init(status);

        //         status.botController = botController;

        //         allPlayerStatuses.Add(status);
        //         alivePlayerStatuses.Add(status);

        //         hudManager.CreatePlayerHeader(status);
        //         botsAdded++;
        //     }

        //     if (AppManager.app.playerTokens[i] != null && AppManager.app.gameSettings.botsOnly)
        //     {
        //         AppManager.app.playerTokens[i].input.SwitchCurrentActionMap("Spectator");
        //     }
        // }
    }
}
