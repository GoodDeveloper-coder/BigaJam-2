using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Speed : PowerUp_Base
{
    [SerializeField] private float _addSpeed = 5f;
    


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        EnableComponents(false);
    
        playerScript.walkSpeed += _addSpeed;
        playerScript.dashSpeed += _addSpeed;
        
        yield return new WaitForSeconds(_Duration);

        playerScript.walkSpeed -= _addSpeed;
        playerScript.dashSpeed -= _addSpeed;        
    }

}
