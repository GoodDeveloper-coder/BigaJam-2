using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> _CurrentInteractedObjects = new List<GameObject>();

    private PlayerMovement _PlayerMovement;



    // Start is called before the first frame update
    void Start()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPowerup(collision, out IPowerUp powerup))
        {
            _CurrentInteractedObjects.Add(collision.gameObject);
            powerup.ActivatePowerUp(_PlayerMovement);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_CurrentInteractedObjects.Contains(collision.gameObject))
            _CurrentInteractedObjects.Remove(collision.gameObject);
    }


    private bool IsPowerup(Collider2D collider, out IPowerUp powerup)
    {
        powerup = collider.GetComponent<IPowerUp>();

        return powerup != null;
    }



    public int InteractingObjectsCount { get { return _CurrentInteractedObjects.Count; } }
}
