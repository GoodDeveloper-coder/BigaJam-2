using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;



public enum PortalDestinationSelectionModes
{
    Random = 0,
    InOrder,
    InOrderBackwards,
}


[Serializable]
public class Activatable_Portal : Activatable_Base
{
    [Header("Portal Settings")]

    [Tooltip("This setting controls how the portal selects a destination when you enter it.")]
    [SerializeField]
    private PortalDestinationSelectionModes _DestinationSelectionMode = PortalDestinationSelectionModes.Random;

    [Tooltip("This setting specifies which teleporter(s) this one will send you to. If multiple destinations are set, it will be random which one you teleport to.")]
    [SerializeField]
    private List<Activatable_Portal> _TeleportDestinations = new List<Activatable_Portal>();


    private List<GameObject> _CurrentInteractingObjects = new List<GameObject>();
    private List<GameObject> _CurrentArrivingObjects = new List<GameObject>();



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


        // If the object that collided with our trigger is one that teleported here from another portal, then ignore it.
        // That way you don't instantly teleport again upon teleporting to a new portal.
        if (_CurrentArrivingObjects.Contains(collision.gameObject))
            return;


        Activate(collision.gameObject);


        if (_TeleportDestinations.Count < 1)
            return;


        Teleport(collision.gameObject, GetDestination());
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them, since player has both.
        if (collision.isTrigger)
            return;


        Deactivate(collision.gameObject);
    }

    public override void Activate(GameObject sender)
    {
        base.Activate(sender);
    }

    public override void Deactivate(GameObject sender)
    {
        base.Deactivate(sender);
    }
    
    public void Receive(GameObject obj)
    {
        // Add it to this list, so we can ignore this object in the collision function.
        // Otherwise you'll keep teleporting from one portal to the next until you hit one with no destination set.
        _CurrentArrivingObjects.Add(obj);

        StartCoroutine(WaitToRemove(obj));
    }


    private void Teleport(GameObject obj, Activatable_Portal destination)
    {
        // Send the game object to the destination portal.
        destination.Receive(obj); // Tell the destination portal what object it is receiving, so it knows not to instantly teleport it again.
        obj.transform.position = destination.transform.position;

        _CurrentInteractingObjects.Remove(obj);
    }

    private Activatable_Portal GetDestination()
    {
        int index = -1;


        if (_DestinationSelectionMode == PortalDestinationSelectionModes.Random)
        {
            index = Random.Range(0, _TeleportDestinations.Count);
        }
        else if (_DestinationSelectionMode == PortalDestinationSelectionModes.InOrder)
        {
            index++;
            if (index >= _TeleportDestinations.Count)
                index = 0;
        }
        else if (_DestinationSelectionMode == PortalDestinationSelectionModes.InOrderBackwards)
        {
            index--;
            if (index < 0)
                index = _TeleportDestinations.Count - 1;
        }


        return _TeleportDestinations[index];
    }

    private IEnumerator WaitToRemove(GameObject obj)
    {
        yield return new WaitForSeconds(1f);

        _CurrentArrivingObjects.Remove(obj);
    }
}
