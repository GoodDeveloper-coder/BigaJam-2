using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_HP : PowerUp_Base
{
    [SerializeField] private float _addHealth = 20f;


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        EnableComponents(false);

        playerScript.PlayerStats.AddHP(_addHealth);

        yield return null;
    }

}
