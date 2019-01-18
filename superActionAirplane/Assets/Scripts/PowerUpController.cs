using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    enum Type {MushroomLSD, SharkNitro};

    [SerializeField]
    Type powerupType = Type.MushroomLSD;

    GameManager gameManager = GameManager.instance;

    void Start()
    {
        ToggleEffectActive();
    }

    public void ToggleEffectActive()
    {
        switch (powerupType)
        {
            case Type.MushroomLSD:
                gameManager.spawnerController.skyController.SetLSD(1);
                gameManager.cameraController.SetLSD(1);
                break;

            case Type.SharkNitro:
                gameManager.spawnerController.SetShark(1);
                gameManager.cameraController.SetShark(1);
                break;
        }
    }

    public void ToggleEffectInactive()
    {
        switch (powerupType)
        {
            case Type.MushroomLSD:
                gameManager.spawnerController.skyController.SetLSD(-1);
                gameManager.cameraController.SetLSD(-1);
             break;

            case Type.SharkNitro:
                gameManager.spawnerController.SetShark(-1);
                gameManager.cameraController.SetShark(-1);
                break;
        }
    }
}