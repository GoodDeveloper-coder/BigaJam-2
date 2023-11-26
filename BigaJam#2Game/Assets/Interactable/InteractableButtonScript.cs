using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButtonScript : MonoBehaviour
{
    [SerializeField] GameObject doorToOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            doorToOpen.GetComponent<Animator>().SetBool("canOpen", true);
            GetComponent<Animator>().SetBool("isPressed", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            doorToOpen.GetComponent<Animator>().SetBool("canOpen", false);
            GetComponent<Animator>().SetBool("isPressed", false);
        }
    }
}
