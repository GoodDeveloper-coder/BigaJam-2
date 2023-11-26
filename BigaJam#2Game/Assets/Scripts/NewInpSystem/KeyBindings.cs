using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public static class KeyBindings
{
    // This is the key we save key bindings under in PlayerPrefs.
    const string KEY_BINDINGS_PLAYER_PREFS_KEY = "KeyBinds";


    public static void LoadKeyBindings(PlayerInput playerInputComponent)
    {
        string keyBindings = PlayerPrefs.GetString(KEY_BINDINGS_PLAYER_PREFS_KEY, string.Empty);

        if (string.IsNullOrWhiteSpace(keyBindings))
            return;

        //Debug.Log("LOADED KEY BINDINGS:  " + keyBindings);

        playerInputComponent.actions.LoadBindingOverridesFromJson(keyBindings);
    }

    public static void SaveKeyBindings(PlayerInput playerInputComponent)
    {
        string keyBindings = playerInputComponent.actions.SaveBindingOverridesAsJson();

        //Debug.Log("SAVED KEY BINDINGS:  " + keyBindings);

        PlayerPrefs.SetString(KEY_BINDINGS_PLAYER_PREFS_KEY, keyBindings);
    }

    public static void ClearAllKeyBindingOverrides()
    {
        // Delete all input binding overrides that are saved in player prefs.
        PlayerPrefs.SetString(KEY_BINDINGS_PLAYER_PREFS_KEY, "");

        // Remove all input binding overrides from the PlayerInput component now, otherwise they might just get saved in player prefs again.
        PlayerInput playerInputComponent = GameObject.FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null )
            playerInputComponent.actions.RemoveAllBindingOverrides();
    }

}
