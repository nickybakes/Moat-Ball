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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
