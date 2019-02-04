using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GjApiManager : MonoBehaviour
{
    void Start()
    {
        if (GameJolt.API.GameJoltAPI.Instance.CurrentUser == null)
        {

        }
    }
}