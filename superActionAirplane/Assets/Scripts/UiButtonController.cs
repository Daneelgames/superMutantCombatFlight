using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public string buttonName = "Roll";

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        if (buttonName == "Roll")
        {
            GameManager.instance.pc.RollByTouch();
        }
        else
        */
        if (buttonName == "Fire_1")
        {
            GameManager.instance.pc.ShootByTouch();
            GameManager.instance.pc.SetCharge(true);
        }
        else if (buttonName == "Invinsible")
        {
            GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.instance.pc.SetCharge(false);
    }

}