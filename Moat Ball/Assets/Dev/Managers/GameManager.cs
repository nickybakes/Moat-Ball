using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tag
{
    Player,
    PickUp,
    UICursor,
    Ball,
    Floor
}

public class GameManager : MonoBehaviour
{
    public static GameManager game;

    public HUDManager hudManager;

    [SerializeField]
    private TempSceneSwap tempSceneSwap;

    public GameObject playerPrefab;
    public GameObject floorColumnPrefab;

    public GameObject ballPrefab;


    [HideInInspector]
    public List<PlayerStatus> allPlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> alivePlayerStatuses;
    [HideInInspector]
    public List<PlayerStatus> eliminatedPlayerStatuses;

    [HideInInspector]
    public List<PlayerStatus> leftTeamPlayerStatuses;

    [HideInInspector]
    public List<PlayerStatus> rightTeamPlayerStatuses;

    private List<Ball> balls;

    public bool dontUpdateGameplay;

    private float columnWidth;

    private float moatCenterX;

    private float leftTeamCenterX;

    public static float LeftTeamCenterX { get => game.leftTeamCenterX; }
    public static float LeftTeamEdge { get => (game.leftTeamColumns[0].transform.position.x + game.columnWidth); }

    private float rightTeamCenterX;
    public static float RightTeamCenterX { get => game.rightTeamCenterX; }
    public static float RightTeamEdge { get => (game.rightTeamColumns[0].transform.position.x - game.columnWidth); }


    private int previousLoserTeam = 0;

    private List<FloorColumn> leftTeamColumns;
    private List<FloorColumn> rightTeamColumns;


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

        leftTeamPlayerStatuses = new List<PlayerStatus>();
        rightTeamPlayerStatuses = new List<PlayerStatus>();

        leftTeamColumns = new List<FloorColumn>();
        rightTeamColumns = new List<FloorColumn>();

        balls = new List<Ball>();

        InstantiateBalls();

        // InitializeCursors();
        // hudManager.cursorPanel.gameObject.SetActive(false);

        // if (AppManager.app.RequestedPlayerAmount == 1)
        //     playersInvincible = true;

        SpawnFloorColumns();
        SpawnPlayerPrefabsSimple();
        RespawnBalls(0);
        // MoveAllPlayersToGround();
        // SpawnRing();

        // StartCoroutine(IntroSequence());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstantiateBalls()
    {
        for (int i = 0; i < AppManager.app.gameSettings.numberOfBalls; i++)
        {
            GameObject g = GameObject.Instantiate(ballPrefab);
            Ball b = g.GetComponent<Ball>();
            balls.Add(b);
        }
    }

    public void EndRound(int winningTeam)
    {
        StartCoroutine(EndRoundProcess(winningTeam));
    }

    private IEnumerator EndRoundProcess(int winningTeam)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        //remove floor columns

        yield return new WaitForSeconds(1);

        RespawnBalls(1 - winningTeam);
    }

    public void RespawnBalls(int previousLoser)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Reset(new Vector3(leftTeamCenterX, 1, 0));
        }
    }

    public void CalculateCenterPoints()
    {
        leftTeamCenterX = -.5f * (AppManager.app.gameSettings.oneSideWidthAtStart - (leftTeamColumns[0].transform.position.x + columnWidth));
        rightTeamCenterX = .5f * (AppManager.app.gameSettings.oneSideWidthAtStart + (rightTeamColumns[0].transform.position.x - columnWidth));
        moatCenterX = .5f * ((rightTeamColumns[0].transform.position.x - columnWidth) + (leftTeamColumns[0].transform.position.x + columnWidth));
        // Debug.Log(leftTeamCenterX + ", " + rightTeamCenterX + ", " + moatCenterX);
    }

    public void SpawnFloorColumns()
    {
        columnWidth = ((float)AppManager.app.gameSettings.oneSideWidthAtStart / (float)AppManager.app.gameSettings.numberOfFloorColumns);

        FloorColumn centerCollider = GameObject.Instantiate(floorColumnPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<FloorColumn>();
        centerCollider.transform.localScale = new Vector3(6, 1, 1);
        centerCollider.RemoveFloorImmediate();

        for (int i = 0; i < AppManager.app.gameSettings.numberOfFloorColumns; i++)
        {
            int flip = -1;
            while (flip <= 1)
            {
                GameObject columnObject = GameObject.Instantiate(floorColumnPrefab, new Vector3(flip * ((3 + columnWidth / 2.0f) + (columnWidth * i)), 0, 0), Quaternion.identity);
                columnObject.transform.localScale = new Vector3(columnWidth, 1, 1);
                if (flip == -1)
                {
                    leftTeamColumns.Add(columnObject.GetComponent<FloorColumn>());
                    leftTeamColumns[leftTeamColumns.Count - 1].pushLeft = true;
                }
                else
                    rightTeamColumns.Add(columnObject.GetComponent<FloorColumn>());

                flip += 2;
            }
        }
    }

    public void RespawnPlayers()
    {
        CalculateCenterPoints();
        float zStart = 3.0f;
        float playerSeparation = 2.0f * zStart / (float)leftTeamPlayerStatuses.Count;
        for (int i = 0; i < leftTeamPlayerStatuses.Count; i++)
        {
            leftTeamPlayerStatuses[i].transform.position = new Vector3(leftTeamCenterX, 1, zStart - (playerSeparation * .5f) - (i * playerSeparation));
            leftTeamPlayerStatuses[i].transform.rotation = Quaternion.Euler(0, 90, 0);
            leftTeamPlayerStatuses[i].gameObject.SetActive(true);
        }
        playerSeparation = 2.0f * zStart / (float)rightTeamPlayerStatuses.Count;
        for (int i = 0; i < rightTeamPlayerStatuses.Count; i++)
        {
            rightTeamPlayerStatuses[i].transform.position = new Vector3(rightTeamCenterX, 1, zStart - (playerSeparation * .5f) - (i * playerSeparation));
            rightTeamPlayerStatuses[i].transform.rotation = Quaternion.Euler(0, -90, 0);
            rightTeamPlayerStatuses[i].gameObject.SetActive(true);
        }

        MoveAllPlayersToGround();
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

                GameObject player = Instantiate(playerPrefab);
                PlayerStatus status = token.SetUpPlayerPrefab(player);
                status.Team = (status.PlayerNumber - 1) % 2;

                if (leftSide)
                    leftTeamPlayerStatuses.Add(status);
                else
                    rightTeamPlayerStatuses.Add(status);

                allPlayerStatuses.Add(status);
                alivePlayerStatuses.Add(status);

                status.gameObject.SetActive(false);

                hudManager.CreatePlayerHeader(status);
            }
        }

        RespawnPlayers();
    }

    public void MoveAllPlayersToGround()
    {
        foreach (PlayerStatus s in allPlayerStatuses)
        {
            for (int i = 0; i < 100; i++)
            {
                s.Movement.UpdateManual(true, false, false, false);
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
