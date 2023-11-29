using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PowerUpCreator : MonoBehaviour
{
    [SerializeField] private float _reloadTime;
    [SerializeField] private GameObject _resObject;

    private void Start()
    {
        InitPowerUp(Instantiate(_resObject, transform.position, Quaternion.identity, transform));
    }

    IEnumerator Respawn() 
    {
        yield return new WaitForSeconds(_reloadTime);
        InitPowerUp(Instantiate(_resObject, transform.position, Quaternion.identity, transform));
    }
    void InitPowerUp(GameObject go)
    {
        IPowerUp powerUp = go.GetComponent<IPowerUp>();
        powerUp.OnPickedUp += () => { StartCoroutine(Respawn()); };
        if (powerUp == null) { Debug.LogError("Can't find IPowerUp"); }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}