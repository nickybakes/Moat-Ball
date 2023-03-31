using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    /// <summary>
    /// starts at 0 (0 = player 1);
    /// </summary>
    public int playerNumber;

    private BasicState currentPlayerState;

    public BasicState CurrentPlayerState { get { return currentPlayerState; } }

    public PlayerMaterialHandler materialHandler;

    private PlayerMovement movement;

    private new Transform transform;


    void Awake()
    {
        transform = gameObject.transform;
        movement = GetComponent<PlayerMovement>();

        movement.Start();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        movement.UpdateManual(true, true, true, false);
    }
}
