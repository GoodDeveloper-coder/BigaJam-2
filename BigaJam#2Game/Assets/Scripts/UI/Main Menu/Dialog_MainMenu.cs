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

    [SerializeField] AudioClip _ButtonClickSound;

    private CutScenePlayer _CutScenePlayer;


    void Awake()
    {
        ButtonClickPlayer.ButtonClickSound = _ButtonClickSound;


        _CutScenePlayer = FindObjectOfType<CutScenePlayer>();
        if (_CutScenePlayer == null)
            Debug.LogError("There is no CutScenePlayer in this scene!");


        PlayerInput playerInputComponent = FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null)
            KeyBindings.LoadKeyBindings(playerInputComponent);
        else
            Debug.LogError("Could not load key bindings as there is no PlayerInput component in this scene!");

        // If PlayerPrefs doesn't have settings values, then write the defaults there now.
        if (!PlayerPrefs.HasKey("Sfx_Volume"))
            DefaultSettingsValues.WriteDefaultValuesToPlayerPrefs();
    }

    private IEnumerator WaitForCutSceneToEnd()
    {
        while (_CutScenePlayer.IsPlaying)
            yield return null;
    }
        
    private IEnumerator StartStoryCutScene(string sceneToLoad)
    {
        _CutScenePlayer.Play();

        yield return StartCoroutine(WaitForCutSceneToEnd());

        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnPlayParkourClicked()
    {
        ButtonClickPlayer.Play();

        StartCoroutine(StartStoryCutScene("ParkourLevel_01"));
    }

    public void OnPlayShootingClicked()
    {
        ButtonClickPlayer.Play();

        StartCoroutine(StartStoryCutScene("ShootingLevel_01 1"));
    }

    public void OnSettingsClicked()
    {
        ButtonClickPlayer.Play();

        this.CloseDialog();

        _SettingsMenu.OpenDialog();
    }

    public void OnExitClicked()
    {
        ButtonClickPlayer.Play();

        Application.Quit();
    }
}

