using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler
{
    public string buttonName = "Invinsible";
    public MenuController menuController;
    bool canPress = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canPress)
        {
            if (buttonName == "Play")
            {
                canPress = false;
                Invoke("CanPress", 1);
                GameManager.instance.GameStart();
                menuController.GameStart();
            }
            else if (buttonName == "SettingsStart")
            {
                canPress = false;
                Invoke("CanPress", 1);
                menuController.SettingsStart();
            }
            else if (buttonName == "SettingsClose")
            {
                canPress = false;
                Invoke("CanPress", 1);
                menuController.SettingsClose();
            }
            else if (buttonName == "Invinsible")
            {
                GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
            }
        }
    }

    void CanPress()
    {
        canPress = true;
    }
}