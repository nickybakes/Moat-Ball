using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public MeshRenderer mesh;

    public GameObject modelHolder;

    public Material material;

    private float speed;

    private Vector3 targetPosition;

    void Start()
    {
        material = new Material(material);
        mesh.material = material;
    }

    void Update()
    {

    }


    private void OnCollisionEnter(Collision other)
    {

    }
}
