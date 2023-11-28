using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [Min(0)]
    [SerializeField] int _MaxAmmo = 100;
    [Min(0)]
    [SerializeField] float _MaxEnergy = 100f;
    [Min(0)]
    [SerializeField] float _MaxHP = 100f;



    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void AddAmmo(int amount)
    {
        if (IsPositive(amount))
            Ammo = Mathf.Clamp(Ammo + amount, 0, _MaxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        if (IsPositive(amount))
            Ammo = Mathf.Clamp(Ammo - amount, 0, _MaxAmmo);
    }

    public void IncreaseMaxAmmoBy(float amount)
    {
        if (IsPositive(amount))
            _MaxHP += amount;
    }


    public void AddEnergy(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(Energy + amount, 0, _MaxEnergy);
    }

    public void RemoveEnergy(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(Energy - amount, 0, _MaxEnergy);
    }

    public void IncreaseMaxEnergyBy(float amount)
    {
        if (IsPositive(amount))
            _MaxEnergy += amount;
    }



    public void AddHP(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(HP + amount, 0, _MaxHP);
    }

    public void RemoveHP(float amount)
    {
        if (IsPositive(amount))
            HP = Mathf.Clamp(HP - amount, 0, _MaxHP);
    }

    public void IncreaseMaxHpBy(float amount)
    {
        if (IsPositive(amount))
            _MaxHP += amount;
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



    public float HP { get; private set; }
    public float Energy { get; private set; }
    public int Ammo { get; private set; }
}
