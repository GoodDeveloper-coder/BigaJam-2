using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Energy : PowerUp_Base
{
    [SerializeField] private float _addEnergy = 20f;


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        EnableComponents(false);

        playerScript.PlayerStats.AddEnergy(_addEnergy);

        yield return null;
    }

}
