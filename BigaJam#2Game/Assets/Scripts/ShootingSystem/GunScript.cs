using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public GameObject target;

    public bool inZone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
}
