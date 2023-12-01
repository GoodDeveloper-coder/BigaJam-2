using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Jump : PowerUp_Base
{
    [SerializeField] private float _addJumpForce = 5f;


    public override IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        EnableComponents(false);

        playerScript.jumpForce += _addJumpForce;
        yield return new WaitForSeconds(_Duration);
        playerScript.jumpForce -= _addJumpForce;
    }

}
