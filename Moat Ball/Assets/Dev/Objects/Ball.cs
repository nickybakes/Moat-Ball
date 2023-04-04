using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public MeshRenderer mesh;

    public TrailRenderer trail;

    public GameObject modelHolder;

    public Material material;

    [SerializeField]
    private GameObject unhittableVFX;

    private bool moveAlongPath;

    private bool unhittable;

    private int allowedBounces;

    private int bounceCount;

    private float speed;

    private float currentSpeed;

    private float pathProgress;

    private List<PlayerStatus> playersThatHaveSet;

    private Vector3 startPosition;

    private Vector2 topDownPosition;

    private Vector3 currentPosition;

    private Vector3 targetPosition;

    private float apexHeight;

    private new Transform transform;

    void Start()
    {
        material = new Material(material);
        mesh.material = material;
        trail.material = material;

        playersThatHaveSet = new List<PlayerStatus>(6);

        allowedBounces = AppManager.app.gameSettings.allowedBounces;

        transform = gameObject.transform;

        currentPosition = new Vector3();
        topDownPosition = new Vector2();
    }

    void Update()
    {
        if (moveAlongPath)
        {
            pathProgress = Mathf.Min(pathProgress + (currentSpeed * Time.deltaTime), 1);
            currentPosition = transform.position;
            currentPosition.x = Mathf.Lerp(startPosition.x, targetPosition.x, pathProgress);
            currentPosition.y = (-(Mathf.Pow((pathProgress * 2 - 1), 2)) + 1) * apexHeight + startPosition.y;
            currentPosition.z = Mathf.Lerp(startPosition.z, targetPosition.z, pathProgress);
            transform.position = currentPosition;

            if (pathProgress >= 1)
            {
                moveAlongPath = false;
            }
        }
    }

    private void SetUnhittable(bool unhittable)
    {
        unhittableVFX.SetActive(unhittable);
        this.unhittable = unhittable;
    }

    public void SetColorByPlayer(PlayerStatus status)
    {
        material.SetColor("_Color_Tint", PlayerToken.colors[status.PlayerNumber - 1]);
    }

    public void SetBall(PlayerStatus status)
    {
        SetColorByPlayer(status);
        pathProgress = 0;
        moveAlongPath = true;
    }

    public void VolleyBall(PlayerStatus status)
    {
        if (unhittable)
        {

            return;
        }
        SetUnhittable(true);
        SetColorByPlayer(status);
        moveAlongPath = true;
        pathProgress = 0;

        targetPosition.x = 5;
        targetPosition.y = 0;
        targetPosition.z = 0;
        apexHeight = 4;
        speed = 1;
        currentSpeed = 1;
        startPosition = transform.position;
        startPosition.y = 0;
    }

    public void Reset()
    {
        speed = 0;
        bounceCount = 0;
        targetPosition = Vector3.zero;
    }


    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.CompareTag(Tag.Floor.ToString()))
        // {

        // }
    }
}
