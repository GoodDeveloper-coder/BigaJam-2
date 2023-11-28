using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class Interactable_Base : Activatable_Base, IInteractable
{
    public void BeginInteraction(GameObject source)
    {
        ActiveInteractionsCount++;
        if (IsActivated)
            return;


        IsBeingInteractedWith = true;

        Activate(source);
    }

    public void EndInteraction(GameObject source)
    {
        ActiveInteractionsCount--;
        if (ActiveInteractionsCount > 0)
            return;

        if (_StayActivatedForever)
            return;
            

        IsBeingInteractedWith = false;

        Deactivate(source);
    }



    public bool IsBeingInteractedWith { get; private set; }
    public int ActiveInteractionsCount { get; private set; }
}
