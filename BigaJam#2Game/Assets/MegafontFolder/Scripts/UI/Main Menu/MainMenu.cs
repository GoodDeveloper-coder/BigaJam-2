using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        Debug.Log("Play clicked.");
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings clicked.");
    }

    public void OnExitClicked()
    {
        Debug.Log("Exit clicked.");

        Application.Quit();
    }
}

