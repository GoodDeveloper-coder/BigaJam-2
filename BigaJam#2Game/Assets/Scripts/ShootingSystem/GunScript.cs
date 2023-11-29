using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Pool))]
public class GunScript : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private InputActionReference _shootKey;
    [SerializeField] private InputActionReference _reloadKey;

    [SerializeField] private int _gunMaxAmmo = 30;
    [SerializeField] private int _gunAmmo = 30;
    [SerializeField] private TextMeshProUGUI _gunAmmoText;

    private GameObject target;

    public bool inZone = false;

    private Pool _pool;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _pool = GetComponent<Pool>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (inZone)
        {
            LookAtTarget();
        }
    }

    private void Awake()
    {
        _shootKey.action.performed += Shoot;
        _reloadKey.action.performed += Reload;
    }

    private void OnDestroy()
    {
        _shootKey.action.performed -= Shoot;
        _reloadKey.action.performed -= Reload;
    }

    //--------------Function for gun following player--------------\\ 
    void LookAtTarget()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Angle = Mathf.Clamp(Angle, -20, 1);

        transform.Rotate(0, 0, Angle);
    }

    //--------------Function for detecting if player is in zone--------------\\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement _pl = collision.GetComponent<PlayerMovement>();

        if (_pl != null)
        {
            target = _pl.gameObject;
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement _pl = collision.GetComponent<PlayerMovement>();

        if (_pl != null)
        {
            inZone = false;
        }
    }

    //--------------Function for shooting--------------\\
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (_gunAmmo > 0)
        {
            _gunAmmo--;
            UpdateGunAmmotText();
            _pool.GetFreeElement(_bulletSpawnPos.position, _bulletSpawnPos.rotation);
            _anim.SetTrigger("Shoot");
        }
    }

    void Reload(InputAction.CallbackContext ctx)
    {
        _gunAmmo = _gunMaxAmmo;
        UpdateGunAmmotText();
    }

    void UpdateGunAmmotText()
    {
        _gunAmmoText.text = $"Ammo: {_gunAmmo}/30";
    }
}
