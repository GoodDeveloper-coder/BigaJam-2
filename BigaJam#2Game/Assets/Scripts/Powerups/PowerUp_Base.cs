using System;
using System.Collections;
using UnityEngine;

public abstract class PowerUp_Base : MonoBehaviour, IPowerUp
{
    public event Action OnPickedUp;

    [SerializeField] protected float _Duration = 5f;
    
    [SerializeField] private bool _DoesRespawn;
    [SerializeField] private float _RespawnTime = 10f;


    private SpriteRenderer _renderer;
    private Collider2D _collider;


    protected void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        // Do not respond to collisions with triggers.
        if (other.isTrigger)
            return;


        var pm = other.gameObject.GetComponent<PlayerMovement>();
        
        if (pm != null && _DoesRespawn) 
            StartCoroutine(ActivatePowerUp(pm));
        
        OnPickedUp?.Invoke();
        StartCoroutine(RespawnPowerUp());
    }

    protected void EnableComponents(bool state)
    {
        _renderer.enabled = state;
        _collider.enabled = state;
    }

    private IEnumerator RespawnPowerUp()
    {
        yield return new WaitForSeconds(_RespawnTime);
        EnableComponents(true);
    }

    public abstract IEnumerator ActivatePowerUp(PlayerMovement playerScript);



    public float Duration 
    { 
        get { return _Duration; } 
        protected set { _Duration = value; }
    }
}