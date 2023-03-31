using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TempSceneSwap : MonoBehaviour
{
    public void InitApp()
    {
        SceneManager.LoadScene((int) Scenes.MENU_InitApp_01);
    }
}
