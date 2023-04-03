using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColumn : MonoBehaviour
{
    [SerializeField]
    private GameObject topCollision;
    [SerializeField]
    private GameObject bottomCollision;
    [SerializeField]
    private PhysicsPush playerPushCollision;

    [SerializeField]
    private Animator animator;

    [HideInInspector]
    public bool pushLeft;

    public void RemoveFloor()
    {
        if (pushLeft)
            playerPushCollision.topDownDirection = new Vector2(-1, 0);
        StartCoroutine(PushPlayersAndRemoveFloor());
    }

    public void RemoveFloorImmediate()
    {
        topCollision.gameObject.SetActive(true);
        bottomCollision.gameObject.SetActive(false);
    }

    private IEnumerator PushPlayersAndRemoveFloor()
    {
        topCollision.gameObject.SetActive(true);
        playerPushCollision.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(.75f);

        playerPushCollision.gameObject.SetActive(false);
        bottomCollision.gameObject.SetActive(false);
    }


}
