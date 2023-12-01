using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;



[Serializable]
public class Activatable_CheckPoint : Activatable_Base
{
    new void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them, since player has both.
        if (collision.isTrigger)
            return;


        // If the object that collided with the checkpoint is a player, then set their last checkpoint.
        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if (pm != null && IsActivated)
            pm.SetCheckPoint(this);
    }

    public override void Activate(GameObject sender)
    {
        base.Activate(sender);
    }

    public override void Deactivate(GameObject sender)
    {
        base.Deactivate(sender);
    }
    
}
