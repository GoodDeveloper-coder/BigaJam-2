using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class Dialog_Base : MonoBehaviour
{  
    public virtual void CloseDialog()
    {
        gameObject.SetActive(false);
    }

    public virtual void OpenDialog()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
    }



    public bool IsOpen {  get { return gameObject.activeSelf; } }
}
