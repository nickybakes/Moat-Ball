using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeader : MonoBehaviour
{
    private PlayerStatus playerStatus;

    private Canvas canvas;

    private Transform tr;

    private Transform playerTransform;

    public TextMeshProUGUI playerNumberText;

    public Image chargeMeterFill;

    private RectTransform chargeMeterFillRect;
    public Image chargeMeterBorder;

    private RectTransform chargeMeterBorderRect;

    public Image chargeMeterBackground;

    private RectTransform chargeMeterBackgroundRect;

    public TextMeshProUGUI stateText;

    public PlayerStatus Status
    {
        get { return playerStatus; }
    }

    public void Setup(PlayerStatus status, Canvas c)
    {
        playerStatus = status;
        canvas = c;
        playerStatus.header = this;
        playerTransform = playerStatus.transform;
        tr = transform;
        playerNumberText.text = "" + playerStatus.PlayerNumber;
        chargeMeterFill.color = PlayerToken.colors[playerStatus.PlayerNumber - 1];

        chargeMeterFillRect = chargeMeterFill.GetComponent<RectTransform>();
        chargeMeterBorderRect = chargeMeterBorder.GetComponent<RectTransform>();
        chargeMeterBackgroundRect = chargeMeterBackground.GetComponent<RectTransform>();

        chargeMeterBackground.gameObject.SetActive(false);
    }

    public void UpdateChargeMeter()
    {
        if (playerStatus.ChargeAmount == 0)
        {
            chargeMeterBackground.gameObject.SetActive(false);
            return;
        }
        else if (!chargeMeterBackground.gameObject.activeSelf)
        {
            chargeMeterBackground.gameObject.SetActive(true);
        }

        float width = chargeMeterBackgroundRect.rect.width;
        float chargePercentage = playerStatus.ChargeAmount / PlayerStatus.chargeMax;

        //DECREASES BARS FROM THE LEFT SIDE
        chargeMeterFillRect.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Left,
            0,
            width * chargePercentage
        );
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(playerTransform.position);
        position.y -= 42 * canvas.scaleFactor;
        tr.position = position;
    }

    public void SetStateText(string text)
    {
        stateText.text = text;
    }

    // public void SetWeaponText(string weaponName)
    // {
    //     if (weaponName == "")
    //     {
    //         weaponText.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         weaponText.text = weaponName;
    //         weaponText.gameObject.SetActive(true);
    //     }
    // }
}
