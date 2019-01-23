using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text score;
    public Animator anim;

    public void GameOver()
    {
        anim.SetTrigger("GameOver");
    }

    public void GameStart()
    {
        anim.SetTrigger("GameStart");
    }

    public void SetCoins(int coins)
    {
        score.text = coins.ToString();
    }
}