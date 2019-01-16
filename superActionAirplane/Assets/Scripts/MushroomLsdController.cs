using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomLsdController : MonoBehaviour
{    
    void Start()
    {
        Invoke("ToggleLsdActive", 0.1f);
    }

    public void ToggleLsdActive()
    {
        GameManager.instance.spawnerController.skyController.SetLSD(1);
        GameManager.instance.cameraController.SetLSD(1);
    }

    public void ToggleLsdInactive()
    {
        GameManager.instance.spawnerController.skyController.SetLSD(-1);
        GameManager.instance.cameraController.SetLSD(-1);
    }
}