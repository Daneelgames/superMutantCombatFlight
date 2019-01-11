using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler //, IPointerUpHandler
{
    public string buttonName = "Roll";
    public Text speedText;
    public Text invincibleText;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonName == "Invinsible")
        {
            GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
        }
        else if (buttonName == "SpeedUp")
        {
            GameManager.instance.pc.touchMovementScaler += 0.1f;
        }
        else if (buttonName == "SpeedDown")
        {
            GameManager.instance.pc.touchMovementScaler -= 0.1f;
        }
    }

    private void Update()
    {
        //if (shoot) GameManager.instance.pc.ShootByTouch();
        if (speedText) speedText.text = "Player speed is " + GameManager.instance.pc.touchMovementScaler;
        if (invincibleText) invincibleText.text = "Player invincible: " + GameManager.instance.pc.invinsible;
    }
}