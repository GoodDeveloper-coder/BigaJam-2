using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Dialog_PauseMenu : Dialog_Base
{
    [SerializeField]
    private Dialog_SettingsMenu _SettingsMenu;

    [SerializeField] InputActionReference _Player1PauseKey;
    [SerializeField] InputActionReference _Player2PauseKey;


    private float _OpenTime = 0;



    void Awake()
    {
        PlayerInput playerInputComponent = FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null)
            KeyBindings.LoadKeyBindings(playerInputComponent);
        else
            Debug.LogError("Could not load key bindings as there is no PlayerInput component in this scene!");


        if (_Player1PauseKey == null)
            Debug.LogError("The player 1 pause key action is not set!");
        if (_Player2PauseKey == null)
            Debug.LogError("The player 2 pause key action is not set!");


        _Player1PauseKey.action.performed += PausePressed;
        _Player2PauseKey.action.performed += PausePressed;
    }

    void OnDestroy()
    {
        _Player1PauseKey.action.performed -= PausePressed;
        _Player2PauseKey.action.performed -= PausePressed;
    }

    void PausePressed(InputAction.CallbackContext ctx)
    {
        // This if statement stops the window instantly closing itself when it sees the same keypress that opened the menu.
        if (Time.time - _OpenTime < 0.1f)
            return;

        CloseDialog();
    }

    public void OnSettingsClicked()
    {
        this.CloseDialog();

        _SettingsMenu.OpenDialog();
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnExitClicked()
    {
        Application.Quit();
    }

    public override void OpenDialog()
    {
        _OpenTime = Time.time;
        base.OpenDialog();
    }
    public override void CloseDialog()
    {
        Cursor.visible = false;
        base.CloseDialog();
    }
}

