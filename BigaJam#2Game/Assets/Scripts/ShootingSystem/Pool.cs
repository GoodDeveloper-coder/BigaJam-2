using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] protected PoolObject _prefab;

    [Space(height: 10)]
    [SerializeField] protected Transform _container;
    [SerializeField] protected int _minCapacity = 100;
    [SerializeField] protected int _maxCapacity = 200;
    
    [Space(height: 10)]
    [SerializeField] protected bool _autoExpand;

    protected List<PoolObject> _pool;



    protected void Awake()
    {

    }

    protected void Start()
    {
        CreatePool();

    }

    protected void OnValidate()
    {
        if (_autoExpand)
        {
            _maxCapacity = Int32.MaxValue;
        }
    }

    protected void CheckPrefab(PoolObject prefab)
    {
        if (prefab == null)
            throw new Exception("Failed to create new Pool because the passed in prefab is null!");
    }

    protected void CreatePool()
    {
        if (_container == null)
            _container = transform;

        // Abort if the pool is already initialized. This will be the case when this function is called by the BulletPool subclass, and then Start() gets called by Unity shortly thereafter.
        if (_pool != null)
            return;


        CheckPrefab(_prefab);


        _pool = new List<PoolObject>(_minCapacity);

        for (int i = 0; i < _minCapacity; i++)
        {
            CreateElement(); //�� �������� ��� ���
        }
    }

    protected virtual PoolObject CreateElement(bool isActiveByDefault = false)
    {
        var createdObject = Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isActiveByDefault);

        _pool.Add(createdObject);

        return createdObject;
    }

    public bool TryGetElement(out PoolObject element)
    {
        foreach (var item in _pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public PoolObject GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        var tr = TryGetTrailRenderer(element.gameObject);
        element.transform.position = position;
        
        if (tr != null)
            tr.Clear();

        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation)
    {
        var element = GetFreeElement(position);
        element.transform.rotation = rotation;
        return element;
    }

    public PoolObject GetFreeElement()
    {
        if (TryGetElement(out var element))
        {
            return element;
        }

        if (_autoExpand)
        {
            return CreateElement(true);
        }

        if (_pool.Count < _maxCapacity)
        {
            return CreateElement(true);
        }

        throw new Exception("Pool is over!");
    }

    TrailRenderer TryGetTrailRenderer(GameObject go)
    {
        var tr = go.GetComponent<TrailRenderer>();
        if (tr != null)
        {
            return tr;
        }
        return null;
    }

}
