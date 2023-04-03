using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public MeshRenderer mesh;

    public TrailRenderer trail;

    public GameObject modelHolder;

    public Material material;

    private bool unhittable;

    private int allowedBounces;

    private int bounceCount;

    private float speed;

    private List<PlayerStatus> playersThaHaveSet;

    private Vector3 targetPosition;

    void Start()
    {
        material = new Material(material);
        mesh.material = material;
        trail.material = material;

        playersThaHaveSet = new List<PlayerStatus>(6);

        allowedBounces = AppManager.app.gameSettings.allowedBounces;
    }

    void Update()
    {

    }

    private bool SetUnhittable(bool unhittable)
    {

    }

    public void SetColorByPlayer(PlayerStatus status)
    {
        material.SetColor("_Color_Tint", PlayerToken.colors[status.PlayerNumber]);
    }

    public void SetBall(PlayerStatus status)
    {
        SetColorByPlayer(status);
    }

    public void VolleyBall(PlayerStatus status)
    {
        SetColorByPlayer(status);
    }

    public void Reset()
    {
        speed = 0;
        bounceCount = 0;
        targetPosition = Vector3.zero;
    }


    private void OnCollisionEnter(Collision other)
    {

    }
}
