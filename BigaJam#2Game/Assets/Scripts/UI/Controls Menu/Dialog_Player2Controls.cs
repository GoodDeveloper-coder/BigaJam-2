using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.UI;


public class Dialog_Player2Controls : Dialog_Base
{
    [SerializeField] private Dialog_ControlsMenu _ControlsMenu;



    public void OnResetAllBindings()
    {
        ButtonClickPlayer.Play();


        RebindActionUI[] rebindActionUIs = GetComponentsInChildren<RebindActionUI>();


        foreach (RebindActionUI rebindUI in rebindActionUIs)
        {
            rebindUI.ResetToDefault(false);
        }

    }

    public void OnReturnToControlsMenuClicked()
    {
        ButtonClickPlayer.Play();


        PlayerInput playerInputComponent = FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null)
            KeyBindings.SaveKeyBindings(playerInputComponent);
        else
            Debug.LogError("Could not save key bindings as there is no PlayerInput component in this scene!");


        CloseDialog();
        _ControlsMenu.OpenDialog();
    }

}

