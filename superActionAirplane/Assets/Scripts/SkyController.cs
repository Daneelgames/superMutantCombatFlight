using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Animator skyAnimator;
    public int lsdCount = 0;

    public void SetLSD(int _lsd)
    {
        lsdCount += _lsd;
        skyAnimator.SetInteger("LSD", lsdCount);
    }
}