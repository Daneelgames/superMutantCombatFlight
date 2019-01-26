using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Vector2 startPos;
    Vector2 currentPos;
    Vector2 distance;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            distance = currentPos - startPos;
        }
    }
}