using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolObject _prefab;

    [Space(height: 10)][SerializeField] Transform _container;
    [SerializeField] private int _minCapacity;
    [SerializeField] private int _maxCapacity = 100;
    [Space(height: 10)][SerializeField] private bool _autoExpand;

    private List<PoolObject> _pool;

    private void Awake()
    {
        CreatePool();

        if (_container = null)
            _container = transform;
    }

    private void Start()
    {

    }

    private void OnValidate()
    {
        if (_autoExpand)
        {
            _maxCapacity = Int32.MaxValue;
        }
    }

    private void CreatePool()
    {
        _pool = new List<PoolObject>(_minCapacity);

        for (int i = 0; i < _minCapacity; i++)
        {
            CreateElement(); //�� �������� ��� ���
        }
    }

    private PoolObject CreateElement(bool isActiveByDefault = false)
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
