using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLivesController : MonoBehaviour
{
    public List<Image> hearts = new List<Image>();

    public void PlayerDamaged()
    {
        int playerLives = GameManager.instance.pc.lives;
        foreach(Image i in hearts)
        {
            if (hearts.IndexOf(i) + 1 > playerLives)
                i.enabled = false;
        }
    }
}