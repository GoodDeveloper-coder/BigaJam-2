using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class Dialog_Base : MonoBehaviour
{ 
    public void CloseDialog()
    {
        gameObject.SetActive(false);
    }

    public void OpenDialog()
    {
        gameObject.SetActive(true);
    }

}
