using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class BulletPoolsCollection : MonoBehaviour 
{
    [SerializeField] PoolObject _BulletPrefab;

    [Header("Pool Settings")]
    [Space(height: 10)]
    [SerializeField] protected Transform _container;
    [SerializeField] protected int _minCapacity = 100;
    [SerializeField] protected int _maxCapacity = 200;

    [Space(height: 10)]
    [SerializeField] protected bool _autoExpand;



    Dictionary<AmmoSO, BulletPool> _PoolsDict = new Dictionary<AmmoSO, BulletPool>();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public BulletPool GetPool(AmmoSO ammoType)
    {
        return GetOrCreatePool(ammoType);
    }

    private BulletPool GetOrCreatePool(AmmoSO ammoType)
    {
        if (_BulletPrefab == null)
            throw new Exception("BulletPoolsCollection failed to create a new pool because its bullet prefab is not set!");
        if (_BulletPrefab == null)
            throw new Exception("BulletPoolsCollection failed to create a new pool because the specified bullet prefab is not a bullet prefab!");
        if (_container == null)
            throw new Exception("BulletPoolsCollection failed to create a new pool because its bullet container is not set!");


        BulletPool pool = null;
        if (_PoolsDict.TryGetValue(ammoType, out pool))
            return pool;


        pool = gameObject.AddComponent<BulletPool>();
        pool.Init(ammoType, _BulletPrefab, _container, _minCapacity, _maxCapacity, _autoExpand);
        _PoolsDict.Add(ammoType, pool);

        return pool;
    }
}
