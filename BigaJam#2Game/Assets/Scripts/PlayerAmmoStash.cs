using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAmmoStash
{
    // Stores a count of how much of each ammo type the player has in their stash.
    Dictionary<AmmoSO, AmmoStash> _AmmoCounts = new Dictionary<AmmoSO, AmmoStash>();



    public int GetAmmoCount(AmmoSO ammoType)
    {
        AmmoStash ammoStash = GetOrCreateAmmoStash(ammoType);

        return ammoStash.Ammo;
    }

    public void AddAmmo(AmmoSO ammoType, int amountToAdd)
    {
        AmmoStash ammoStash = GetOrCreateAmmoStash(ammoType);
        ammoStash.Ammo = Mathf.Clamp(ammoStash.Ammo + amountToAdd, 0, ammoStash.MaxAmmo);
    }

    public void RemoveAmmo(AmmoSO ammoType, int amountToRemove)
    {
        AmmoStash ammoStash = GetOrCreateAmmoStash(ammoType);
        ammoStash.Ammo = Mathf.Clamp(ammoStash.Ammo - amountToRemove, 0, ammoStash.MaxAmmo);
    }

    public void IncreaseMaxAmmo(AmmoSO ammoType, int amountToAdd)
    {
        if (amountToAdd <= 0)
            throw new Exception("The amount to increase max ammo by for this ammo stash must be positive!");

        AmmoStash ammoStash = GetOrCreateAmmoStash(ammoType);
        ammoStash.MaxAmmo += amountToAdd;
    }

    private AmmoStash GetOrCreateAmmoStash(AmmoSO ammoType)
    {
        if (_AmmoCounts.TryGetValue(ammoType, out AmmoStash ammoStash))
            return ammoStash;


        return AddNewAmmoStash(ammoType);
    }

    private AmmoStash AddNewAmmoStash(AmmoSO ammoType)
    {
        AmmoStash ammoStash = new AmmoStash();
        ammoStash.Ammo = ammoType.StartingAmmo;
        ammoStash.MaxAmmo = ammoType.MaxAmmo;


        // Add the new ammo stash to the dictionary.
        _AmmoCounts.Add(ammoType, ammoStash);

        return ammoStash;
    }



    private class AmmoStash
    {
        public int Ammo = 0;
        public int MaxAmmo = 100;
    }
}
