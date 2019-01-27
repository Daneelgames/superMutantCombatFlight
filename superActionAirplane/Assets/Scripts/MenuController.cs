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

    public AudioSource guiActorAudioSource;
    public Animator guiActorAnimator;
    int guiActorCurrentLine = 0;
    public List<AudioClip> guiActorLines;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void GameOver()
    {
        anim.SetTrigger("GameOver");
    }

    public void GameStart()
    {
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

    public IEnumerator GuiActorPlay(string reason)
    {
        float playTime = .75f;
        if (reason == "WaveDestroyed")
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
            guiActorAnimator.SetTrigger("ActorLeft");
        else
            guiActorAnimator.SetTrigger("ActorRight");

        guiActorAnimator.SetBool("Sleep", false);

        yield return new WaitForSecondsRealtime(playTime);

        guiActorAnimator.SetBool("Sleep", true);
    }
}