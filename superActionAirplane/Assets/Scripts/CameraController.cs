using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator anim;

    Vector3 newEulerAngles = Vector3.zero;
    int lsdCount = 0;
    int sharkCount = 0;

    public void SetLSD(int _lsd)
    {
        lsdCount += _lsd;
        anim.SetInteger("LSD", lsdCount);
    }

    public void SetShark(int _shark)
    {
        sharkCount += _shark;
        anim.SetInteger("Shark", sharkCount);
    }
}