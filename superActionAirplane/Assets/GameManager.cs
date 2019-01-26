﻿using UnityEngine;
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
        audio.pitch = 0.5f;
    }

    public void GameStart()
    {
        pc.transform.position = Vector3.zero;
        pc.gameObject.SetActive(true);
        pc.SetSensitivity(menuController.playersSensitivitySlider.value);
        spawnerController.StartSpawning();
        cameraController.SetMenu(false);
        //    spawnerController.movementSpeed = 100;
    }

    public void Restart()
    {
        objectPooler.DisableAllProjectiles();
        spawnerController.StopSpawning();
        menuController.GameOver();
        spawnerController.ClearWaves();
        cameraController.SetMenu(true);
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
}