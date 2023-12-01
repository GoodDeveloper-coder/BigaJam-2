using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> _CurrentInteractedObjects = new List<GameObject>();

    private PlayerMovement _PlayerMovement;

    void Start()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPowerup(collision, out List<IPowerUp> powerups))
        {
            _CurrentInteractedObjects.Add(collision.gameObject);
            
            // Activate all powerups that are on the object we hit.
            foreach (IPowerUp powerup in powerups)
                powerup.ActivatePowerUp(_PlayerMovement);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_CurrentInteractedObjects.Contains(collision.gameObject))
            _CurrentInteractedObjects.Remove(collision.gameObject);
    }


    private bool IsPowerup(Collider2D collider, out List<IPowerUp> powerups)
    {        
        powerups = collider.GetComponents<IPowerUp>().ToList();

        return powerups.Count > 0;
    }

    public int InteractingObjectsCount { get { return _CurrentInteractedObjects.Count; } }
}
