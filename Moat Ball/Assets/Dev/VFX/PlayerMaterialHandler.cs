using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialHandler : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] meshes;

    [SerializeField]
    private Material material;

    public void SetMaterialColorByIndex(int playerNumber)
    {
        material = new Material(material);
        material.SetColor("_Color_Tint", PlayerToken.colors[playerNumber - 1]);
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = material;
        }
    }
}
