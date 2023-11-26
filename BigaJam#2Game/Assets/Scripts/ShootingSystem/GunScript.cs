using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] CheckTriggerZone ck;

    [SerializeField] GameObject player;

    private bool _isPlayerCollision;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        ck._isPlayerCollision = this._isPlayerCollision;
        if (_isPlayerCollision)
        {
            this.transform.LookAt(player.transform);
        }
    }

}
