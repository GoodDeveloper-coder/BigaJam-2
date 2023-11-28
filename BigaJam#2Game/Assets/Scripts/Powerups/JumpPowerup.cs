using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour, IPowerUp
{
    [field: SerializeField] public float Duration { get; set; }
    [SerializeField] private float _addJumpForce;

    public IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        SetOffComponents();
        float _defaultJumpForce = playerScript.jumpForce;
        playerScript.jumpForce += _addJumpForce;
        yield return new WaitForSeconds(Duration);
        playerScript.jumpForce = _defaultJumpForce;
        Destroy(gameObject);
    }
    void SetOffComponents()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var pm = other.gameObject.GetComponent<PlayerMovement>();
        if (pm != null) StartCoroutine(ActivatePowerUp(pm));
    }
}
