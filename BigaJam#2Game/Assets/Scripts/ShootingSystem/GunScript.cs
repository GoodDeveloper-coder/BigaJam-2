using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Pool))]
public class GunScript : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private InputActionReference _shootKey;

    public GameObject target;

    public bool inZone = false;

    private Pool _pool;

    // Start is called before the first frame update
    void Start()
    {
        _pool = GetComponent<Pool>();
    }

    void Update()
    {
        if (inZone)
        {
            LookAtTarget();
        }
    }

    private void Awake()
    {
        _shootKey.action.performed += Shoot;
    }

    private void OnDestroy()
    {
        _shootKey.action.performed -= Shoot;
    }

    //--------------Function for gun following player--------------\\ 
    void LookAtTarget()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Angle = Mathf.Clamp(Angle, -20, 1);

        transform.Rotate(0, 0, Angle);
    }

    //--------------Function for detecting if player is in zone--------------\\
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            inZone = false;
        }
    }

    //--------------Function for shooting--------------\\
    void Shoot(InputAction.CallbackContext ctx)
    {
        _pool.GetFreeElement(_bulletSpawnPos.position, _bulletSpawnPos.rotation);
    }
}
