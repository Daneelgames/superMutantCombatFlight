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

    public void GameStart()
    {
        if (canPress)
        {
            canPress = false;
            Invoke("CanPress", 1);
            GameManager.instance.GameStart();
            menuController.GameStart();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canPress)
        {
            if (buttonName == "Play")
            {
                GameStart();
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
            else if (buttonName == "DisclamerYes")
            {
                canPress = false;
                menuController.DisclamerAnswer(true);
            }
            else if (buttonName == "DisclamerNo")
            {
                canPress = false;
                menuController.DisclamerAnswer(false);
            }
        }
    }

    void CanPress()
    {
        canPress = true;
    }
}