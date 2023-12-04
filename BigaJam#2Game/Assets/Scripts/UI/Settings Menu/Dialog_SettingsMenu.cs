using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Dialog_SettingsMenu : Dialog_Base
{
    [SerializeField] private Dialog_MainMenu _MainMenu;
    [SerializeField] private Dialog_PauseMenu _PauseMenu;

    [Header("Settings Pages")]
    [SerializeField] private Dialog_GameplaySettings _GameplaySettings;
    [SerializeField] private Dialog_ControlsMenu _ControlsMenu;
    [SerializeField] private Dialog_AudioSettings _AudioSettings;



    public void OnGameplayClicked()
    {
        ButtonClickPlayer.Play();

        this.CloseDialog();
        _GameplaySettings.OpenDialog();
    }

    public void OnControlsClicked()
    {
        ButtonClickPlayer.Play();

        this.CloseDialog();
        _ControlsMenu.OpenDialog();
    }

    public void OnAudioClicked()
    {
        ButtonClickPlayer.Play();

        this.CloseDialog();
        _AudioSettings.OpenDialog();
    }

    public void OnReturnToMainMenuClicked()
    {
        ButtonClickPlayer.Play();

        this.CloseDialog();


        if (_MainMenu != null)
            _MainMenu.OpenDialog();
        else if (_PauseMenu != null)
            _PauseMenu.OpenDialog();
        else
            Debug.LogError("Can't return to previous menu because it hasn't been set!");
    }

}

