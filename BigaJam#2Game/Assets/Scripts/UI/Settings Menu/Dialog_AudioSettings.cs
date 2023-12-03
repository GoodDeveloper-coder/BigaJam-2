using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class Dialog_AudioSettings : Dialog_Base
{
    [SerializeField]
    private Dialog_SettingsMenu _SettingsMenu;


    private MusicManager _MusicManager;
    private Slider _SfxVolumeSlider;
    private Slider _MusicVolumeSlider;


    void Awake()
    {
        _MusicManager = FindObjectOfType<MusicManager>();
        if (_MusicManager == null)
            Debug.LogError("There is no MusicManager in this scene!");

        _SfxVolumeSlider = GetSlider("Slider_Sfx");
        _MusicVolumeSlider = GetSlider("Slider_Music");

        GetSfxSettings();
    }

    public void OnReturnToSettingsMenuClicked()
    {       
        SaveSfxSettings();

        this.CloseDialog();
        _SettingsMenu.OpenDialog();
    }

    public void OnResetAllClicked()
    {
        _SfxVolumeSlider.value = DefaultSettingsValues.SfxVolume;
        _MusicVolumeSlider.value = DefaultSettingsValues.MusicVolume;
    }

    private Slider GetSlider(string gameObjectName)
    {
        GameObject go= GameObject.Find(gameObjectName);
        if (go == null)
            Debug.LogError("Failed to find the sound effects slider!");


        Slider slider = go.GetComponent<Slider>();
        if (go == null)
            Debug.LogError("The sound effects slider object has no slider component on it!");


        return slider;
    }

    public void OnSfxVolumeChanged(float newVolume)
    {

    }

    public void OnMusicVolumeChanged(float newVolume)
    {
        
    }

    private void GetSfxSettings()
    {
        _SfxVolumeSlider.value = PlayerPrefs.GetFloat("Sfx_Volume");
        _MusicVolumeSlider.value = PlayerPrefs.GetFloat("Music_Volume");
    }
    private void SaveSfxSettings()
    {
        PlayerPrefs.SetFloat("Sfx_Volume", _SfxVolumeSlider.value);
        PlayerPrefs.SetFloat("Music_Volume", _MusicVolumeSlider.value);
    }

        
}

