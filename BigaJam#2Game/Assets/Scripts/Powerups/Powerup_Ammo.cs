using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup_Ammo : PowerUp_Base
{
    [SerializeField] private AmmoSO _AmmoType;
    [SerializeField] private int _addAmmo = 20;


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        if (_AmmoType == null)
            throw new Exception("PowerUp failed to add ammo, because its AmmoType is not set!");


        EnableComponents(false);

        playerScript.PlayerStats.AddAmmo(_AmmoType, _addAmmo);

        yield return null;
    }

}
