using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLivesController : MonoBehaviour
{
    public List<Image> hearts = new List<Image>();

    public void UpdateLivesUI()
    {
        int playerLives = GameManager.instance.pc.lives;

        for (int i = 0; i < hearts.Count; i ++)
        {
            if (i + 1 > playerLives)
                hearts[i].enabled = false;
            else if (i + 1 == playerLives)
                hearts[i].enabled = true;
        }
    }
}