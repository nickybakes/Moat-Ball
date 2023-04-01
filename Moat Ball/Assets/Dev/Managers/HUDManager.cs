using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    private static HUDManager hud;

    public GameObject headerPanel;

    public GameObject headerPrefab;

    [HideInInspector]
    public List<PlayerHeader> headers;

    public Canvas canvas;
    public RectTransform canvasRect;


    // Start is called before the first frame update
    void Start()
    {
        hud = this;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CreatePlayerHeader(PlayerStatus status)
    {
        GameObject headerObject = Instantiate(headerPrefab, headerPanel.transform);
        PlayerHeader header = headerObject.GetComponent<PlayerHeader>();
        headers.Add(header);
        header.Setup(status, canvas);
    }

    public void RemoveHeader(PlayerStatus status)
    {
        PlayerHeader header = null;
        foreach (PlayerHeader h in headers)
        {
            if (h.Status == status)
            {
                header = h;
            }
        }

        if (header != null)
        {
            headers.Remove(header);
            header.gameObject.SetActive(false);
        }
    }
}
