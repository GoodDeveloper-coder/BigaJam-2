using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableGun : MonoBehaviour
{
    [SerializeField] private GunSO _gun;
    [SerializeField] private float _lifetime = 0.3f;

    private SpriteRenderer _sr;
    private Collider2D _collider;

    private void Awake() {
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
    private void Start() {
        _sr.sprite = _gun.gunSprite;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        var pm = other.gameObject.GetComponent<PlayerMovement>();
        
        if (pm != null) 
            StartCoroutine(PickGun(pm));
    }
    private IEnumerator PickGun(PlayerMovement pm)
    {
        pm.Gun.AddGun(_gun);
        _collider.enabled = false;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < _lifetime)
        {
            currentTime += Time.deltaTime;
            transform.localScale = 
                Vector3.Lerp(startScale, endScale, currentTime / _lifetime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
