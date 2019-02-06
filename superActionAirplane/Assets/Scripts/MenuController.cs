using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text score;
    public Animator anim;
    public Slider playersSensitivitySlider;
    public Slider musicVolumeSlider;
    public Toggle toggleAcidSky;
    public InputField inputField;

    public AudioSource guiActorAudioSource;
    public Animator guiActorAnimator;
    int guiActorCurrentLine = 0;
    public List<AudioClip> guiActorLines;
    public List<AudioClip> guiWeaponLines;

    public UiButtonController playButton;

    public GjApiManager gjApi;

    GameManager gameManager;
    public List<GameObject> blackBoarders;

    private void Start()
    {
        foreach(GameObject obj in blackBoarders)
        {
            obj.SetActive(true);
        }
        gameManager = GameManager.instance;
        score.gameObject.SetActive(false);
    }

    public void ClearScore()
    {
        score.text = "0";
    }

    public void InputScore()
    {
        anim.SetTrigger("InputScore");
        inputField.Select();
    }

    public void MainMenu()
    {
        anim.SetTrigger("MainMenu");
    }

    public void GameStart()
    {
        score.gameObject.SetActive(true);
        anim.SetTrigger("GameStart");
    }

    public void SetCoins(int coins)
    {
        score.text = coins.ToString();
    }

    public void SettingsStart()
    {
        anim.SetTrigger("SettingsStart");
        gameManager.SetCanStartGame(false);
    }

    public void SettingsClose()
    {
        anim.SetTrigger("SettingsClose");
        gameManager.SetCanStartGame(true);
    }

    public void ChangeMusicVolume()
    {
        gameManager.SetMusicVolume(musicVolumeSlider.value);
    }

    public void  ToggleAcidSky()
    {
        gameManager.SetAcidSkyEnabled(toggleAcidSky.isOn);
    }

    public IEnumerator GuiActorPlay(string reason, string actorName)
    {
        float playTime = .75f;

        int index = 0;

        if (reason == "PowerUp" && guiWeaponLines.Count > 0)
        {
            switch (actorName)
            {
                case "AdditionalShotgun":
                    index = 0;
                    break;

                case "AdditionalMiniGun":
                    index = 1;
                    break;

                case "AdditionalNitroShark":
                    index = 2;
                    break;

                case "AdditionalSniperRifle":
                    index = 3;
                    break;

                case "AdditionalPie":
                    index = 4;
                    break;

                case "AdditionalHandPistol":
                    index = 5;
                    break;

                case "AdditionalMushroomLSD":
                    index = 6;
                    break;

                case "AdditionalRocketShooter":
                    index = 7;
                    break;
            }
            guiActorAudioSource.clip = guiWeaponLines[index];
        }
        else if (reason == "WaveDestroyed")
        {
            guiActorAudioSource.clip = guiActorLines[Random.Range(2, guiActorLines.Count)];
        }
        else if (reason == "FirstWave")
        {
            guiActorAudioSource.clip = guiActorLines[0];
        }
        else if (reason == "GameOver")
        {
            guiActorAudioSource.clip = guiActorLines[1];
            playTime = 2;
        }

        guiActorAudioSource.Play();

        if (gameManager.pc.transform.position.x > 0)
        {
            guiActorAnimator.transform.localScale = new Vector3(1, 1, 1);
        }
        else
            guiActorAnimator.transform.localScale = new Vector3(-1, 1, 1);

        guiActorAnimator.SetTrigger(actorName);

        guiActorAnimator.SetBool("Sleep", false);

        yield return new WaitForSecondsRealtime(playTime);

        guiActorAnimator.SetBool("Sleep", true);
    }

    public void DisclamerAnswer(bool answer)
    {
        gameManager.SetAcidSkyEnabled(answer);
        anim.SetTrigger("Disclamer");
        gameManager.cameraController.StartMusic();

        toggleAcidSky.isOn = answer;
        gameManager.SetCanStartGame(true);
    }

    public string GetInputText()
    {
        return inputField.text;
    }

    public void ShowLeaderboard()
    {
        anim.SetTrigger("ShowLeaderboard");
    }
}