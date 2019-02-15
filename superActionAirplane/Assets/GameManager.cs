using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;       //Allows us to use Lists. 
using GameJolt;
using GameJolt.UI;
using GameJolt.API;

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

    public GjApiManager gjApiManager;

    public AudioSource audio;

    public bool playerAlive = false;

    bool canStartGame = false;
    bool settings = false;
    public bool inputScore = false;
    bool leaderboard = false;
    bool canPause = false;
    public GameObject pause;

    public GameJolt.UI.Controllers.LeaderboardsWindow leaderboardsWindow;

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
        if (Input.GetButtonDown("Jump") && canStartGame && !leaderboard && !settings)
        {
            menuController.playButton.GameStart();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            if (playerAlive)
            {
                if (Time.timeScale != 0)
                {
                    pause.SetActive(true);
                    Time.timeScale = 0;
                    Invoke("CanPause", 0.25f * Time.unscaledDeltaTime);
                    cameraController._audio.Pause();
                }
                else if (Time.timeScale == 0)
                {
                    pause.SetActive(false);
                    Time.timeScale = 1;
                    Invoke("CanPause", 0.25f * Time.unscaledDeltaTime);
                    cameraController._audio.UnPause();
                }
            }
            else if (inputScore)
            {
                ShowLeaderboard();
            }
            else if (leaderboard)
                HideLeaderboard();
            else if (settings)
                MainMenu();
        }
        else if (Input.GetButtonDown("Submit") && inputScore)
        {
            InputScoreOk();
        }
    }

    public void InputScoreOk()
    {
        if (menuController.GetInputText().Length > 0)
            SetScore(false);
        else
            SetScore(true);
    }

    void CanPause()
    {
        if (playerAlive)
            canPause = true;
    }

    private void Start()
    {
        //    spawnerController.movementSpeed = 0;
        pc.gameObject.SetActive(false);
        audio.pitch = 0.5f;
    }

    public void GameStart()
    {
        canPause = true;
        CancelInvoke();
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
    }

    public void Restart()
    {
        canPause = false;
        playerAlive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        spawnerController.skyController.skyAnimator.SetBool("Gameplay", false);
        //StartCoroutine(menuController.GuiActorPlay("GameOver", "Player"));
        objectPooler.DisableAllProjectiles();
        spawnerController.StopSpawning();
        spawnerController.ClearWaves();
        cameraController.SetMenu(true);
        
        GameJolt.API.Scores.GetRank(coins, 0, (int rank) => {
            Debug.Log(string.Format("Rank {0}", rank));

            if (rank <= 25)
            {
                if (GameJoltAPI.Instance.CurrentUser == null)
                    InputScore();
                else
                {
                    gjApiManager.SaveScore(coins, "");
                    Invoke("ShowLeaderboardDelayed", 1);
                }
            }
            else
                MainMenu();
        });
    }

    void InputScore()
    {
        menuController.InputScore();
        //GameJoltUI.Instance.ShowLeaderboards();
        inputScore = true;
    }

    public void HideLeaderboard()
    {
        MainMenu();
    }

    void MainMenu()
    {
        inputScore = false;
        settings = false;
        leaderboard = false;
        menuController.MainMenu();
        Invoke("SetCanStart", 1);
        leaderboardsWindow.Dismiss(false);
    }

    public void ShowLeaderboard()
    {
        inputScore = false;
        leaderboard = true;
        GameJoltUI.Instance.ShowLeaderboards();
        menuController.ShowLeaderboard();
    }

    void SetCanStart()
    {
        canStartGame = true;
    }

    void SetScore(bool anonim)
    {
        inputScore = false;
        menuController.ShowLeaderboard();
        if (anonim)
            gjApiManager.SaveScore(coins, "Unknown");
        else
            gjApiManager.SaveScore(coins, menuController.GetInputText());

        Invoke("ShowLeaderboardDelayed", 1);
    }

    void ShowLeaderboardDelayed()
    {
        GameJoltUI.Instance.ShowLeaderboards();
        leaderboard = true;
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
        settings = !active;
    }
}