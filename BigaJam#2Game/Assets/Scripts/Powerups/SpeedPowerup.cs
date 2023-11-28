using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : MonoBehaviour, IPowerUp
{
    [field: SerializeField] public float Duration { get; set; }
    [SerializeField] private float _addSpeed;

    public IEnumerator ActivatePowerUp(PlayerMovement playerScript)
    {
        SetOffComponents();
        float _defaultSpeed = playerScript.speed;
        playerScript.speed += _addSpeed;
        yield return new WaitForSeconds(Duration);
        playerScript.speed = _defaultSpeed;
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