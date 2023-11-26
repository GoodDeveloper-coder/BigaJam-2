using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


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

    }

    public void OnPlayParkourClicked()
    {
        Debug.Log("Play Parkour clicked.");
    }

    public void OnPlayShootingClicked()
    {
        Debug.Log("Play Shooting clicked.");
    }

    public void OnSettingsClicked()
    {
        this.CloseDialog();

        _SettingsMenu.OpenDialog();
    }

    public void OnExitClicked()
    {
        Debug.Log("Exit clicked.");

        Application.Quit();
    }
}

