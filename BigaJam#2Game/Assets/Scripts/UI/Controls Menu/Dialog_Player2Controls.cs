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
        RebindActionUI[] rebindActionUIs = GetComponentsInChildren<RebindActionUI>();


        foreach (RebindActionUI rebindUI in rebindActionUIs)
        {
            Button resetButton = rebindUI.transform.Find("ResetToDefaultButton").transform.Find("ResetButton").GetComponent<Button>();

            resetButton.onClick.Invoke();
        }

    }

    public void OnReturnToControlsMenuClicked()
    {
        PlayerInput playerInputComponent = FindObjectOfType<PlayerInput>();
        if (playerInputComponent != null)
            KeyBindings.SaveKeyBindings(playerInputComponent);
        else
            Debug.LogError("Could not save key bindings as there is no PlayerInput component in this scene!");


        this.CloseDialog();
        _ControlsMenu.OpenDialog();
    }

}

