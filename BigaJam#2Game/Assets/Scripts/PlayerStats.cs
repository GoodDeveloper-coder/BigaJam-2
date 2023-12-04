using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    #region Fields
    [Header("Max Values")]
    [SerializeField] bool _HasInfiniteAmmo = false;
    [Min(0)]
    [SerializeField] float _MaxEnergy = 100f;
    [Min(0)]
    [SerializeField] float _MaxHP = 100f;

    [Header("Functionality")]
    [Tooltip("How much HP regenerates per second.")]
    [SerializeField] private float _HpRegenRate = 2f;
    [Tooltip("How much energy regenerates per second.")]
    [SerializeField] private float _EnergyRegenRate = 2f;

    [Header("UI")]
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Image _hpImage;
    [SerializeField] private Image _energyImage;
    [SerializeField] private GameObject _winSecondPlayerMenu;

    [Header("Choose regime parkour or shooting")]
    [SerializeField] private bool _isShootingRegime;
    [SerializeField] private bool _isParkourRegime;

    [Header("Sounds")]
    [SerializeField] private AudioSource _dieSound;

    private PlayerMovement _PlayerMovement;
    private GunScript _GunScript;


    #endregion
    #region Properties

    public float HP { get; private set; }
    public float Energy { get; private set; }
    public PlayerAmmoStash AmmoStash { get; private set; } = new PlayerAmmoStash();

    #endregion
    #region MonoBehaviour Methods


    void Start()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();
        _GunScript = _PlayerMovement.Gun;

        AddHP(_MaxHP);
        AddEnergy(_MaxEnergy);

        CheckAmmoUI();
        CheckEnergyUI();
        CheckHPUI();
    }

    void Update()
    {
        if (HP > 0)
        {
            if (HP < _MaxHP)
                RegenHealth();
            if (Energy < _MaxEnergy)
                RegenEnergy();
        }
    }

    #endregion
    #region Methods

    void CheckHPUI() { _hpImage.fillAmount = (float)HP / _MaxHP; }
    void CheckEnergyUI() { _energyImage.fillAmount = (float)Energy / _MaxEnergy; }
    public void CheckAmmoUI()
    {
        _ammoText.text = $"{_GunScript.CurrentGun.Ammo}/{AmmoStash.GetAmmoCount(_GunScript.CurrentGun.AmmoType)}"; 
    }

    void RegenHealth()
    {
        // Heal _HpRegenRate health per second.
        HP = Mathf.Clamp(HP + (_HpRegenRate * Time.deltaTime), 0f, _MaxHP);
        CheckHPUI();
    }

    void RegenEnergy()
    {
        // Restore _EnergyRegenRate energy per second.
        Energy = Mathf.Clamp(Energy + (_EnergyRegenRate * Time.deltaTime), 0f, _MaxEnergy);
        CheckEnergyUI();
    }

    public void AddAmmo(AmmoSO ammoType, int amount)
    {
        if (IsPositive(amount))
        {
            AmmoStash.AddAmmo(ammoType, amount);
        }

        CheckAmmoUI();
    }

    public void RemoveAmmo(AmmoSO ammoType, int amount)
    {
        if (IsPositive(amount))
            AmmoStash.RemoveAmmo(ammoType, amount);

        CheckAmmoUI();
    }

    public void ReloadGun(GunScript gunScript)
    {
        _GunScript = gunScript;
        GunSO gunSO = gunScript.CurrentGun;

        if (gunSO.Ammo >= gunSO.MaxGunAmmo)
        {
            CheckAmmoUI();
            return;
        }



        int curAmmoCount = AmmoStash.GetAmmoCount(gunSO.AmmoType);
        int amountToReload = gunSO.MaxGunAmmo - gunSO.Ammo;

        if (amountToReload >= curAmmoCount)
            amountToReload = curAmmoCount;


        if (!_HasInfiniteAmmo && !gunSO.AmmoType.HasInfiniteAmmo)
            AmmoStash.RemoveAmmo(gunSO.AmmoType, amountToReload);

        gunSO.Ammo += amountToReload;

        CheckAmmoUI();
    }

    public void IncreaseMaxAmmoBy(AmmoSO ammoType, int amount)
    {
        if (IsPositive(amount))
            AmmoStash.IncreaseMaxAmmo(ammoType, amount);

        CheckAmmoUI();
    }

    public void AddEnergy(float amount)
    {
        if (IsPositive(amount))
            Energy = Mathf.Clamp(Energy + amount, 0, _MaxEnergy);
        CheckEnergyUI();
    }

    public void RemoveEnergy(float amount)
    {
        if (IsPositive(amount))
            Energy = Mathf.Clamp(Energy - amount, 0, _MaxEnergy);
        CheckEnergyUI();
    }

    public void IncreaseMaxEnergyBy(float amount)
    {
        if (IsPositive(amount))
            _MaxEnergy += amount;
        CheckEnergyUI();
    }

    public void AddHP(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(HP + amount, 0, _MaxHP);
        CheckHPUI();
    }

    public void RemoveHP(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(HP - amount, 0, _MaxHP);
        CheckHPUI();
        CheckWinOrLoose();
    }

    public void IncreaseMaxHpBy(float amount)
    {
        if (IsPositive(amount))
            _MaxHP += amount;
        CheckHPUI();
    }

    private bool IsPositive(float amount)
    {
        if (amount < 0)
            Debug.LogError("Amount must be positive!");

        return amount >= 0;
    }

    private bool IsPositive(int amount)
    {
        if (amount < 0)
            Debug.LogError("Amount must be positive!");

        return amount >= 0;
    }

    private void CheckWinOrLoose()
    {
        if (HP <= 0)
        {
            if (_isShootingRegime)
            {
                _winSecondPlayerMenu.SetActive(true);
                GetComponent<Animator>().SetBool("Die", true);
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerStats>().enabled = false;
                _dieSound.Play();
                Invoke("LockTime", 1f);
            }
            else if (_isParkourRegime)
            {
                Debug.Log("@#%$#^$%&^");
                HP = _MaxHP;
                _PlayerMovement.WarpToLastCheckPoint();
            }
        }
    }

    void LockTime()
    {
        GetComponent<Collider2D>().enabled = false;
        Time.timeScale = 0f;
    }

    public bool HasInfiniteAmmo { get { return _HasInfiniteAmmo; } }

    #endregion
}
