using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTextureController : MonoBehaviour
{
    public float speed = 50;
    public Material mat;
    public float resetOn = -1000;

    private void Update()
    {
        mat.mainTextureOffset = new Vector2(0, mat.mainTextureOffset.y - Time.deltaTime * speed);

        if (mat.mainTextureOffset.y < resetOn)
            mat.mainTextureOffset = Vector2.zero;

    }
}
