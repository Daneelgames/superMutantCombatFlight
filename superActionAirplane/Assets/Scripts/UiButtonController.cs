using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string buttonName = "Roll";
    bool shoot = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonName == "Fire_1")
        {
            shoot = true;
        }
        else if (buttonName == "Invinsible")
        {
            GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
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
            shoot = false;
        }
    }

    private void Update()
    {
        if (shoot) GameManager.instance.pc.ShootByTouch();
    }
}