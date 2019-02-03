﻿using System.Collections;
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

    public AudioSource guiActorAudioSource;
    public Animator guiActorAnimator;
    int guiActorCurrentLine = 0;
    public List<AudioClip> guiActorLines;
    public List<AudioClip> guiWeaponLines; 

     GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        score.gameObject.SetActive(false);
    }

    public void ClearScore()
    {
        score.text = "0";
    }

    public void GameOver()
    {
        anim.SetTrigger("GameOver");
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
    }

    public void SettingsClose()
    {
        anim.SetTrigger("SettingsClose");
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
}