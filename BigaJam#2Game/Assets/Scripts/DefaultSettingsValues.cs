using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultSettingsValues
{
    public const float SfxVolume = 1.0f;
    public const float MusicVolume = 0.5f;



    public static void WriteDefaultValuesToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("Sfx_Volume", SfxVolume);
        PlayerPrefs.SetFloat("Music_Volume", MusicVolume);
    }
}
