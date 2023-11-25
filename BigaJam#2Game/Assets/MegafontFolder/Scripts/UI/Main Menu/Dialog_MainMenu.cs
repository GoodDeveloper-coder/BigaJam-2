using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Dialog_MainMenu : Dialog_Base
{
    [SerializeField]
    private Dialog_SettingsMenu _SettingsMenu;



    public void OnPlayClicked()
    {
        Debug.Log("Play clicked.");
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

