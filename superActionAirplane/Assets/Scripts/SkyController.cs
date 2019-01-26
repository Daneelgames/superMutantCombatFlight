using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Animator skyAnimator;
    public int lsdCount = 0;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void SetLSD(int _lsd)
    {
        lsdCount += _lsd;

        if (gameManager.acidSkyCanBeEnabled)
            skyAnimator.SetInteger("LSD", lsdCount);
    }
}