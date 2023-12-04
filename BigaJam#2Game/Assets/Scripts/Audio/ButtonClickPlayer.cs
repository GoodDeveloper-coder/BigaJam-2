using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButtonClickPlayer
{
    // This field gets set by Dialog_MainMenu.cs
    public static AudioClip ButtonClickSound;


    // We need to play the sound at the camera's position so it isn't too quiet.
    private static Vector3 _CameraPos = new Vector3(0, 0, -10);


    public static void Play()
    {
        Debug.Log(PlayerPrefs.GetFloat("Sfx_Volume"));
        AudioSource.PlayClipAtPoint(ButtonClickSound, _CameraPos, PlayerPrefs.GetFloat("Sfx_Volume"));
    }
}
