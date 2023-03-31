using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TempSceneSwap tempSceneSwap;

    // Start is called before the first frame update
    void Start()
    {
        if (AppManager.app == null)
        {
            tempSceneSwap.gameObject.SetActive(true);
            tempSceneSwap.InitApp();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
