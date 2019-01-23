using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    public int coins = 0;

    public static GameManager instance = null;
    public PlayerController pc;
    public SpawnerController spawnerController;
    public TouchInputController touchInputController;
    public UiLivesController uiLivesController;
    public CameraController cameraController;
    public ObjectPooler objectPooler;

    public MenuController menuController;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.SetResolution(540, 960, true);
        //Camera.main.aspect = 16f / 9f;

        instance = this;
    }

    private void Start()
    {
        //    spawnerController.movementSpeed = 0;
        pc.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        pc.transform.position = Vector3.zero;
        pc.gameObject.SetActive(true);
        spawnerController.StartSpawning();
        //    spawnerController.movementSpeed = 100;
    }

    public void Restart()
    {
        objectPooler.DisableAllProjectiles();
        spawnerController.StopSpawning();
        menuController.GameOver();
        spawnerController.ClearWaves();
    }

    public void AddCoins(int newCoins)
    {
        coins += newCoins;
        menuController.SetCoins(coins);
    }
}