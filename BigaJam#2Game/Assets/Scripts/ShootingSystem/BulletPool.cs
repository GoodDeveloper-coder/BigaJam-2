using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletPool : Pool
{
    private AmmoSO _AmmoType;



    protected override PoolObject CreateElement(bool isActiveByDefault = false)
    {
        _prefab.GetComponent<Bullet>().SetAmmoType(_AmmoType);

        var createdObject = Instantiate(_prefab, _container);
        Bullet bullet = createdObject.GetComponent<Bullet>();
        if (bullet == null)
            throw new Exception("Failed to create new bullet instance because the prefab does not have a Bullet component!");

        createdObject.gameObject.SetActive(isActiveByDefault);

        _pool.Add(createdObject);

        return createdObject;
    }

    public void Init(AmmoSO ammoType, PoolObject prefab, Transform container, int minCapacity, int maxCapacity, bool autoExpand)
    {
        if (ammoType == null)
            throw new Exception("Failed to create new BulletPool because the passed in ammo type is null!");
        if (container == null)
            throw new Exception("Failed to create new BulletPool because the passed in container is null!");


        _AmmoType = ammoType;
        _prefab = prefab;
        _container = container;
        _minCapacity = minCapacity;
        _maxCapacity = maxCapacity;
        _autoExpand = autoExpand;

        // Tell the base class to set up the pool now.
        CreatePool();
    }
    
}
