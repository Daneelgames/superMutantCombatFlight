using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDropFeedbackController : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 3);
    }
}