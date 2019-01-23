using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler
{
    public string buttonName = "Invinsible";
    public MenuController menuController;
    public Image heartsBackground;
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
            else if (buttonName == "Invinsible")
            {
                GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
                heartsBackground.enabled = !heartsBackground.isActiveAndEnabled;
            }
        }
    }

    void CanPress()
    {
        canPress = true;
    }
}