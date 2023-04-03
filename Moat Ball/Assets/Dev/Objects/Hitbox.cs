using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private MeshRenderer mesh;

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

    }
}
