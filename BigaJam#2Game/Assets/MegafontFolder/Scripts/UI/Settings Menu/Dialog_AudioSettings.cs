using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Dialog_AudioSettings : Dialog_Base
{
    [SerializeField]
    private Dialog_SettingsMenu _SettingsMenu;



    public void OnReturnToSettingsMenuClicked()
    {
        this.CloseDialog();
        _SettingsMenu.OpenDialog();
    }

}

