using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BulletPoolsCollection))]
public class GunScript : MonoBehaviour
{
    [SerializeField] private Bullet _BulletPrefab;
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private InputActionReference _shootKey;
    [SerializeField] private InputActionReference _reloadKey;
    [SerializeField] private InputActionReference _changeWeaponKey;
    [SerializeField] private Transform _rotateGun;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<GunSO> _gunsList;
    public bool inZone = false;

    private bool _canReload = true;
    private bool _canShoot = true;
    private int _gunIndex;

    private GameObject target;
    private BulletPoolsCollection _BulletPools;
    private Animator _anim;
    private SpriteRenderer _sp;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();

        _BulletPools = GetComponent<BulletPoolsCollection>();

        _shootKey.action.performed += Shoot;
        _reloadKey.action.performed += Reload;
        _changeWeaponKey.action.performed += ChangeGun;

        InitAllGuns();
        _gunIndex = 0;
        _playerStats.ReloadGun(this);
        _sp.sprite = CurrentGun.gunSprite;
    }

    void Start()
    {
        
   }

    void Update()
    {
        if (inZone)
        {
            LookAtTarget();
        }
    }
   
    private void OnDestroy()
    {
        _shootKey.action.performed -= Shoot;
        _reloadKey.action.performed -= Reload;
    }

    private void InitAllGuns()
    {
        for (int i = 0; i < _gunsList.Count; i++)
            InitGun(_gunsList[i]);
    
    }

    private void InitGun(GunSO gun)
    {
        gun.Ammo = gun.StartingAmmo;
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

        _sp.sprite = CurrentGun.gunSprite;
        _playerStats.ReloadGun(this);
        _anim.runtimeAnimatorController = CurrentGun.animator;
    }
    public void AddGun(GunSO gun)
    {
        InitGun(gun);
        _gunsList.Add(gun);
    }
    //--------------Function for gun following player--------------\\ 
    void LookAtTarget()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Angle = Mathf.Clamp(Angle, -20, 1);

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
        if (CurrentGun.Ammo > 0 && _canShoot)
        {
            CurrentGun.Ammo--;
            _BulletPools.GetPool(CurrentGun.AmmoType).GetFreeElement(_bulletSpawnPos.position, _bulletSpawnPos.rotation);
            _canShoot = false;
            StartCoroutine(ShootCooldown());
            _anim.SetTrigger("Shoot");
            _playerStats.CheckAmmoUI();
        }
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(CurrentGun.Cooldown);
        _canShoot = true;
    }

    void Reload(InputAction.CallbackContext ctx)
    {
        if (_canReload)
        {
            _playerStats.ReloadGun(this);
            _canReload = false;
            Invoke("UnlockReload", 5.0f);
        }
    }

    void UnlockReload()
    {
        _canReload = true;
    }

    public GunSO CurrentGun
    {
        get { return _gunsList[_gunIndex]; }
    }
    
}
