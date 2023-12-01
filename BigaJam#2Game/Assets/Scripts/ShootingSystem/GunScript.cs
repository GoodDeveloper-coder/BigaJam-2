using System;
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
    [SerializeField] private InputActionReference _changeWeaponKey;
    [SerializeField] private Transform _rotateGun;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<GunSO> _gunsList;
    public bool inZone = false;

    private int _gunAmmo;
    private int _gunMaxAmmo;
    private bool _canReload = true;
    private bool _canShoot = true;
    private int _gunIndex;
    private float _shootCooldown;

    private GameObject target;
    private Pool _pool;
    private Animator _anim;
    private SpriteRenderer _sp;

    void Start()
    {
        _pool = GetComponent<Pool>();
        _anim = GetComponent<Animator>();
        _playerStats.AddAmmo(_gunMaxAmmo);
        _sp = GetComponent<SpriteRenderer>();
        _gunAmmo = _gunMaxAmmo;
        
        _gunIndex = 0;
        _sp.sprite = _gunsList[0].gunSprite;
        _gunMaxAmmo = _gunsList[0].MaxGunAmmo;
        _shootCooldown = _gunsList[0].Cooldown;
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
        _changeWeaponKey.action.performed += ChangeGun;
    }

    private void OnDestroy()
    {
        _shootKey.action.performed -= Shoot;
        _reloadKey.action.performed -= Reload;
    }

    public void ChangeGun(InputAction.CallbackContext ctx)
    {
        if (_gunIndex < _gunsList.Count - 1)
        {
            _gunIndex++;
        }
        else
        {
            _gunIndex = 0;
        }
        GunSO newGun = _gunsList[_gunIndex];
        _sp.sprite = newGun.gunSprite;
        _gunMaxAmmo = newGun.MaxGunAmmo;
        _shootCooldown = newGun.Cooldown;
    }
    //--------------Function for gun following player--------------\\ 
    void LookAtTarget()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        //Angle = Mathf.Clamp(Angle, -20, 1);

        _rotateGun.Rotate(0, 0, Angle);
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
        if (_playerStats.Ammo > 0 && _canShoot)
        {
            _gunAmmo--;
            _pool.GetFreeElement(_bulletSpawnPos.position, _bulletSpawnPos.rotation);
            _canShoot = false;
            StartCoroutine(ShootCooldown());
            //_anim.SetTrigger("Shoot");
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(_shootCooldown);
        _canShoot = true;
    }

    void Reload(InputAction.CallbackContext ctx)
    {
        if (_canReload)
        {
            if (_playerStats.HasInfiniteAmmo || _playerStats.Ammo >= _gunMaxAmmo)
            {
                _gunAmmo = _gunMaxAmmo;
                _playerStats.RemoveAmmo(_gunMaxAmmo);
            }
            else if (_playerStats.Ammo > 0)
            {
                _gunAmmo = _playerStats.Ammo;
                _playerStats.RemoveAmmo(_playerStats.Ammo);
            }

            _playerStats.AddAmmo(_gunMaxAmmo);
            _canReload = false;
            Invoke("UnlockReload", 5.0f);
        }
    }

    void UnlockReload()
    {
        _canReload = true;
    }

}
