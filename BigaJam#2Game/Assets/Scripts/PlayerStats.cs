using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    #region Fields
    [Header("Max Values")]
    [Min(0)]
    [SerializeField] int _MaxAmmo = 100;
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

    #endregion
    #region Properties

    public float HP { get; private set; }
    public float Energy { get; private set; }
    public int Ammo { get; private set; }

    #endregion
    #region MonoBehaviour Methods
    void Start()
    {
        AddAmmo(_MaxAmmo);
        AddHP(_MaxHP);
        AddEnergy(_MaxEnergy);

        CheckAmmoUI();
        CheckEnergyUI();
        CheckHPUI();
    }

    void Update()
    {
        if (HP < _MaxHP)
            RegenHealth();
        if (Energy < _MaxEnergy)
            RegenEnergy();
    }

    #endregion
    #region Methods

    void CheckHPUI() { _hpImage.fillAmount = (float)HP / _MaxHP; }
    void CheckEnergyUI() { _energyImage.fillAmount = (float)Energy / _MaxEnergy; }
    void CheckAmmoUI() { _ammoText.text = $"{Ammo.ToString()}/30"; }

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

    public void AddAmmo(int amount)
    {
        if (IsPositive(amount))
        {
            Ammo = Mathf.Clamp(Ammo + amount, 0, _MaxAmmo);
            //Debug.Log(Ammo);
        }
        CheckAmmoUI();
    }

    public void RemoveAmmo(int amount)
    {
        if (IsPositive(amount))
            Ammo = Mathf.Clamp(Ammo - amount, 0, _MaxAmmo);
            //Debug.Log(Ammo);
        CheckAmmoUI();
    }

    public void IncreaseMaxAmmoBy(int amount)
    {
        if (IsPositive(amount))
            Ammo += amount;
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

    #endregion
}
