using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtPlayer : MonoBehaviour
{

    private GameObject target;

    public bool inZone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inZone)
        {
            LookAtTarget();
        }
    }

    void LookAtTarget()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        Angle = Mathf.Clamp(Angle, -20, 1);

        transform.Rotate(0, 0, Angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement _pl = collision.GetComponent<PlayerMovement>();

        if (_pl != null)
        {
            target = _pl.gameObject;
            inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement _pl = collision.GetComponent<PlayerMovement>();

        if (_pl != null)
        {
            inZone = false;
        }
    }
}
