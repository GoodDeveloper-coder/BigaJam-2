using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Dialog_SettingsMenu : Dialog_Base
{
    [SerializeField] private Dialog_MainMenu _MainMenu;

    [Header("Settings Pages")]
    [SerializeField] private Dialog_GameplaySettings _GameplaySettings;
    [SerializeField] private Dialog_ControlsMenu _ControlsMenu;
    [SerializeField] private Dialog_AudioSettings _AudioSettings;



    public void OnGameplayClicked()
    {
        this.CloseDialog();
        _GameplaySettings.OpenDialog();
    }

    public void OnControlsClicked()
    {
        this.CloseDialog();
        _ControlsMenu.OpenDialog();
    }

    public void OnAudioClicked()
    {
        this.CloseDialog();
        _AudioSettings.OpenDialog();
    }

    public void OnReturnToMainMenuClicked()
    {
        this.CloseDialog();
        _MainMenu.OpenDialog();
    }

}

