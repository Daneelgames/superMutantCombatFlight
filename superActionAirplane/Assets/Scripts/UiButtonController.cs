using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler
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
        }
        else if (buttonName == "Invinsible")
        {
            GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
        }
    }
}