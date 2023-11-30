using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "CutSceneContent", menuName = "CutScenes/Content Asset")]
[Serializable]
public class CutSceneContent : ScriptableObject
{
    [Header("Cutscene Intro/Outro Settings")]

    [Tooltip("How long it takes (in seconds) the cutscene GUI to appear when a cutscene begins.")]
    [Min(-1f)]
    public float CutsceneFadeInTime;

    [Tooltip("How long it takes (in seconds) the cutscene GUI to disappear when a cutscene ends.")]
    [Min(-1f)]
    public float CutsceneFadeOutTime;


    [Header("Controls")]

    [Tooltip("Whether or not the player can skip the entire cutscene by invoking InputAction specified by the SkipInputAction setting. If no InputAction is set for skipping, then it's the same as setting this option to false.")]
    public bool CanSkipCutScene = true;

    [Tooltip("Specifies which InputAction should cause the cutscene to be skipped entirely. If this is left null, then the cutscene can not be skipped.")]
    public InputActionReference SkipCutSceneInputAction;


    [Header("Audio Settings")]

    [Tooltip("The volume level of the background music.")]
    [Range(0f, 1f)]
    public float BackgroundMusicVolume;

    [Tooltip("The background music of this cutscene. If you specify multiple tracks, they will play one after the other until the cutscene ends. If theres is only one track, it will repeat until the cutscene ends. Lastly, you can also have a cutscene with no music by leaving this list empty.")]
    public List<AudioClip> BackgroundMusic;


    [Header("Default Page Display Settings")]

    [Tooltip("How long it takes (in seconds) pages to fade in by default. Any page can override this value by setting a positive value for this setting in its page content.")]
    [Min(0f)]
    public float DefaultPageFadeInDuration;

    [Tooltip("How long it takes (in seconds) pages to fade out by default. Any page can override this value by setting a positive value for this setting in its page content.")]
    [Min(0f)]
    public float DefaultPageFadeOutDuration;

    [Tooltip("How long each page is displayed for (in seconds) by default before it starts to transition out. Any page can override this value by setting a positive value for this setting in its page content.")]
    [Min(0f)]
    public float DefaultPageDisplayTime;


    [Tooltip("The default page display settings for this cutscene. Any page can override this by setting its own DisplaySettings option to something other than null.")]
    public CutScenePageDisplaySettings DefaultDisplaySettings;


    [Header("Cutscene Content")]
    [Tooltip("The content for each page in the cutscene.")]
    public List<CutScenePageContent> Pages;



    private ListDefaultValuer<CutScenePageContent> _DefaultValuer;



    public CutSceneContent()
    {
        CutsceneFadeInTime = 1f;
        CutsceneFadeOutTime = 1f;

        BackgroundMusicVolume = 1f;

        DefaultPageFadeInDuration = 1f;
        DefaultPageFadeOutDuration = 1f;
        DefaultPageDisplayTime = 10f;

        DefaultDisplaySettings = null;
    }

    private void OnValidate()
    {
        //ListDefaultValuer<CutScenePageContent>.OnValidate(ref _DefaultValuer, Pages, DefaultValuer.Modes.INITIALISE_DEFAULTS_ON_EVERY_NEW_INSTANCE_ADDED);
    }
}

