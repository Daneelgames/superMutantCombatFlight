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
}