using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Ammo : PowerUp_Base
{
    [SerializeField] private int _addAmmo = 20;


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        EnableComponents(false);

        playerScript.PlayerStats.AddAmmo(_addAmmo);

        yield return null;
    }

}
