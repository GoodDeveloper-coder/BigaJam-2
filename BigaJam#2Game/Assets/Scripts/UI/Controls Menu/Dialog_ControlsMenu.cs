using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Dialog_ControlsMenu : Dialog_Base
{
    [SerializeField] private Dialog_SettingsMenu _SettingsMenu;

    [Header("Controls Pages")]
    [SerializeField] private Dialog_Player1Controls _Player1Controls;
    [SerializeField] private Dialog_Player2Controls _Player2Controls;



    public void OnPlayer1Clicked()
    {
        this.CloseDialog();
        _Player1Controls.OpenDialog();
    }

    public void OnPlayer2Clicked()
    {
        this.CloseDialog();
        _Player2Controls.OpenDialog();
    }

    public void OnReturnToSettingsMenuClicked()
    {
        this.CloseDialog();
        _SettingsMenu.OpenDialog();
    }

}

