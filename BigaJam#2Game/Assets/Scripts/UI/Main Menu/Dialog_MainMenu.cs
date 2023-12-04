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
        
    private IEnumerator StartStoryCutScene(LevelTypes levelType)
    {
        _CutScenePlayer.Play();

        yield return StartCoroutine(WaitForCutSceneToEnd());

        LevelManager.LoadRandomLevel(levelType);
    }

    public void OnPlayParkourClicked()
    {
        ButtonClickPlayer.Play();
        LevelManager.LoadRandomLevel(LevelTypes.Parkour);
        StartCoroutine(StartStoryCutScene(LevelTypes.Parkour));
    }

    public void OnPlayShootingClicked()
    {
        ButtonClickPlayer.Play();

        StartCoroutine(StartStoryCutScene(LevelTypes.Shooting));
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

