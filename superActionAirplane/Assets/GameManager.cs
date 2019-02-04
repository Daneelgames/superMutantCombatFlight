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
    public bool acidSkyCanBeEnabled = true;

    public AudioSource audio;

    public bool playerAlive = false;

    bool canStartGame = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        //Screen.fullScreen = false;
        //Screen.SetResolution(720, 1280, false);
        //Camera.main.aspect = 16f / 9f;

        instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && canStartGame)
        {
            menuController.playButton.GameStart();
        }
    }

    private void Start()
    {
        //    spawnerController.movementSpeed = 0;
        pc.gameObject.SetActive(false);
        audio.pitch = 0.5f;
    }

    public void GameStart()
    {
        canStartGame = false;
        pc.transform.position = Vector3.zero;
        pc.gameObject.SetActive(true);
        spawnerController.skyController.skyAnimator.SetBool("Gameplay", true);
        pc.SetSensitivity(menuController.playersSensitivitySlider.value);
        spawnerController.ClearWaves();
        spawnerController.StartSpawning();
        cameraController.SetMenu(false);
        objectPooler.DisableAllProjectiles();
        playerAlive = true;
        coins = 0;
        menuController.ClearScore();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        //    spawnerController.movementSpeed = 100;
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(menuController.GuiActorPlay("GameOver", "Player"));
        spawnerController.skyController.skyAnimator.SetBool("Gameplay", false);
        playerAlive = true;
        objectPooler.DisableAllProjectiles();
        spawnerController.StopSpawning();
        menuController.GameOver();
        spawnerController.ClearWaves();
        cameraController.SetMenu(true);
        Invoke("SetCanStart", 1);
    }

    void SetCanStart()
    {
        canStartGame = true;
    }

    public void AddCoins(int newCoins)
    {
        coins += newCoins;
        menuController.SetCoins(coins);
    }

    public void SetMusicVolume(float volume)
    {
        audio.volume = volume;
    }

    public void SetAcidSkyEnabled(bool active)
    {
        acidSkyCanBeEnabled = active;
    }

    public void SetCanStartGame(bool active)
    {
        canStartGame = active;
    }
}