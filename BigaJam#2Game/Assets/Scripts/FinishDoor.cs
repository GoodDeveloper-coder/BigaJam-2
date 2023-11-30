using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] private GameObject _firstPlayerWinMenu;
    [SerializeField] private GameObject _secondPlayerWinMenu;

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
        var pm = collision.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            if (pm._playerIndex == 1)
            {
                _firstPlayerWinMenu.SetActive(true);
                Debug.Log("First player won!");
            }

            if (pm._playerIndex == 2)
            {
                _secondPlayerWinMenu.SetActive(true);
                Debug.Log("Second player won!");
            }
        }
    }
}
