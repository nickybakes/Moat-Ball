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

    private Vector2 topDownDirection;

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
        topDownDirection = new Vector2();
    }

    void Update()
    {
        if (moveAlongPath)
        {
            pathProgress = Mathf.Min(pathProgress + (currentSpeed * Time.deltaTime), 1);
            currentPosition = transform.position;
            currentPosition.x = Mathf.Lerp(startPosition.x, targetPosition.x, pathProgress);
            currentPosition.y = (-(Mathf.Pow((pathProgress * 2 - 1), 2)) + 1) * apexHeight + Mathf.Lerp(startPosition.y, targetPosition.y, pathProgress);
            currentPosition.z = Mathf.Lerp(startPosition.z, targetPosition.z, pathProgress);
            transform.position = currentPosition;

            if (pathProgress >= 1)
            {
                LandOnFloor();
            }
        }
    }

    private void LandOnFloor()
    {
        bounceCount++;

        if (bounceCount > allowedBounces)
        {
            //then this player loses!
            if (transform.position.x > 0)
            {
                TestHitBack();
            }
            else
            {
                GameManager.game.EndRound(1);
                return;
            }
        }
        else
        {
            //first check that we actually landed on the floor
            if (transform.position.x > 0)
            {
                //right side but in water
                if (transform.position.x < GameManager.RightTeamEdge)
                {
                    if (bounceCount > 0)
                    {
                        GameManager.game.EndRound(0);
                        return;
                    }
                    else
                    {
                        GameManager.game.EndRound(1);
                        return;
                    }
                }
            }
            else
            {
                if (transform.position.x > GameManager.LeftTeamEdge)
                {
                    if (bounceCount > 0)
                    {
                        GameManager.game.EndRound(1);
                        return;
                    }
                    else
                    {
                        GameManager.game.EndRound(0);
                        return;
                    }
                }
            }
            //bounce!
            SetBounceTarget();
        }
    }

    private void SetBounceTarget()
    {
        float bounceDistance = 1.5f * (bounceCount / allowedBounces);
        targetPosition.x = targetPosition.x + topDownDirection.x * bounceDistance;
        targetPosition.z = targetPosition.z + topDownDirection.y * bounceDistance;
        currentSpeed = speed * .75f;
        apexHeight = 4;

        startPosition = transform.position;

        pathProgress = 0;
        SetUnhittable(false);
        moveAlongPath = true;
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
        if (playersThatHaveSet.Contains(status))
        {
            //hit stun them here

            return;
        }
        SetColorByPlayer(status);
        pathProgress = 0;
        moveAlongPath = true;
        playersThatHaveSet.Add(status);
        SetBounceTarget();
    }

    private void TestHitBack()
    {
        targetPosition.x = Random.Range(-13, GameManager.LeftTeamEdge);
        targetPosition.z = Random.Range(-5, 5);
        apexHeight = 4;

        SetValuesOnHit();
    }

    public void VolleyBall(PlayerStatus status)
    {
        if (unhittable)
        {
            return;
        }
        SetColorByPlayer(status);

        if (status.Team == 0)
        {
            targetPosition.x = 5;
        }
        else
        {
            targetPosition.x = -5;
        }

        targetPosition.z = 0;
        apexHeight = 4;
        SetValuesOnHit();
        playersThatHaveSet.Clear();

    }

    private void SetValuesOnHit()
    {
        SetUnhittable(true);

        targetPosition.y = 0;

        moveAlongPath = true;
        pathProgress = 0;

        bounceCount = 0;

        speed += .05f;
        currentSpeed = speed;
        startPosition = transform.position;

        topDownDirection.x = targetPosition.x - startPosition.x;
        topDownDirection.y = targetPosition.z - startPosition.z;

        topDownDirection.Normalize();
    }

    public void Reset(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        SetUnhittable(false);
        moveAlongPath = false;
        speed = 1;
        bounceCount = 0;
        pathProgress = 0;
        gameObject.transform.position = spawnPosition;
        startPosition = spawnPosition;
        targetPosition = spawnPosition;
    }


    private void OnCollisionEnter(Collision other)
    {
        // if (other.gameObject.CompareTag(Tag.Floor.ToString()))
        // {

        // }
    }
}
