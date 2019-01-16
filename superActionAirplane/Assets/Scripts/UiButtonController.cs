using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler //, IPointerUpHandler
{
    public string buttonName = "Invinsible";
    public Image heartsBackground;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonName == "Invinsible")
        {
            GameManager.instance.pc.invinsible = !GameManager.instance.pc.invinsible;
            heartsBackground.enabled = !heartsBackground.isActiveAndEnabled;
        }
    }
}