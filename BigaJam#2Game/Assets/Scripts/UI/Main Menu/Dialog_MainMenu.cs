using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Dialog_MainMenu : Dialog_Base
{
    [SerializeField]
    private Dialog_SettingsMenu _SettingsMenu;


    private void Awake()
    {
        PlayerInput playerInputComponent = FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null)
            KeyBindings.LoadKeyBindings(playerInputComponent);
        else
            Debug.LogError("Could not load key bindings as there is no PlayerInput component in this scene!");

        // If PlayerPrefs doesn't have settings values, then write the defaults there now.
        if (!PlayerPrefs.HasKey("Sfx_Volume"))
            DefaultSettingsValues.WriteDefaultValuesToPlayerPrefs();
    }

    public void OnPlayParkourClicked()
    {
        SceneManager.LoadScene("ParkourLevel_01");
    }

    public void OnPlayShootingClicked()
    {
        SceneManager.LoadScene("ShootingLevel_01 1");
    }

    public void OnSettingsClicked()
    {
        this.CloseDialog();

        _SettingsMenu.OpenDialog();
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}

