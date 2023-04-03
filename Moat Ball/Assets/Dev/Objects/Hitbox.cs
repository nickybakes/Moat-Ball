using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private MeshRenderer mesh;

    private PlayerStatus status;

    public PlayerStatus Status
    {
        set => status = value;
    }

    public void SetMaterialColor(int playerNumber)
    {
        material = new Material(material);
        material.SetColor("_Color_Tint", PlayerToken.colors[playerNumber - 1]);
        mesh.material = material;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Ball.ToString()))
        {
            Ball ball = other.GetComponent<Ball>();
            status.DetectBallInHitbox(ball);
        }
    }
}
