using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTriggerZone : MonoBehaviour
{
    [SerializeField] GunScript _gunScript;

    private int _currentPlayer;

    public bool _isPlayerCollision;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerCollision)
        {
            Debug.Log("Rotating");
            if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Shoot!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement check = collision.gameObject.GetComponent<PlayerMovement>();
        if (check.PlayerIndex == _currentPlayer)
        {
            Debug.Log("intersect had been 1");
            _isPlayerCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement check = collision.gameObject.GetComponent<PlayerMovement>();
        if (check.PlayerIndex == _currentPlayer)
        {
            Debug.Log("intersect had been 2");
            _isPlayerCollision = false;
        }
    }

    public void SetPlayer(int playerIndex)
    {
        _currentPlayer = playerIndex;
    }
}
