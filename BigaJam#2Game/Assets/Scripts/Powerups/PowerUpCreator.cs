using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Pool))]
public class PowerUpCreator : MonoBehaviour
{
    [SerializeField] private float _respawnTime = 10f;
    [SerializeField] private GameObject _resObject;   
    
    private Pool _pool;
    private PoolObject _lastObjSpawned;


    private void Start()
    {
        _pool = GetComponent<Pool>();
        SpawnPowerup();
    }

    IEnumerator Respawn() 
    {
        yield return new WaitForSeconds(_respawnTime);
        SpawnPowerup();
    }

    private void SpawnPowerup()
    {
        _lastObjSpawned = _pool.GetFreeElement(transform.position, transform.rotation);
        _lastObjSpawned.GetComponent<IPowerUp>().OnPickedUp += OnPickedUp;
    }

    private void OnPickedUp()
    {
        _lastObjSpawned.GetComponent<IPowerUp>().OnPickedUp -= OnPickedUp;
        StartCoroutine(Respawn());
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}