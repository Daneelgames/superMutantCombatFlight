using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiButtonController : MonoBehaviour, IPointerDownHandler //, IPointerUpHandler
{
    public string buttonName = "Roll";
    //bool shoot = false;
    public Text speedText;
    public Text invincibleText;
    public Animator skyAnim;

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        if (buttonName == "Fire_1")
        {
            shoot = true;
            GameManager.instance.pc.ShotAnim(true);
        }
        */

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
        else if (buttonName == "Sky")
        {
            if (skyAnim.speed < 1) skyAnim.speed = 1;
            else if (skyAnim.speed > 0) skyAnim.speed = 0;
        }
    }

    /*
    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonName == "Fire_1")
        {
            shoot = false;
            GameManager.instance.pc.ShotAnim(false);
        }
    }
    */
    
    private void Update()
    {
        //if (shoot) GameManager.instance.pc.ShootByTouch();
        if (speedText) speedText.text = "Player speed is " + GameManager.instance.pc.touchMovementScaler;
        if (invincibleText) invincibleText.text = "Player invincible: " + GameManager.instance.pc.invinsible;
    }
}