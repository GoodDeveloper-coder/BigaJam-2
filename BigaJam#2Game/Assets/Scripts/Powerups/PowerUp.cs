using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Choose only one powerup")]
    [SerializeField] private bool _jumpPowerUp;

    [SerializeField] private bool _speedPowerUp;

    [SerializeField] private bool _speedAndJumpPowerUp;

    [Header("Choose duration of powerup")]
    [SerializeField] private float _durationOfJumpPowerUp = 5f;

    [SerializeField] private float _durationOfSpeedPowerUp = 5f;

    [SerializeField] private float _durationOfSpeedAndJumpPowerUp = 5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement _pl = collision.GetComponent<PlayerMovement>();
        if ( _pl != null)
        {
            if (_jumpPowerUp)
            {
                StartCoroutine(jumpPowerUp(_pl));
                SetOffComponents();
            }

            if (_speedPowerUp)
            {
                StartCoroutine(speedPowerUp(_pl));
                SetOffComponents();
            }

            if (_speedAndJumpPowerUp)
            {
                speedAndJumpPowerUp(_pl);
                StartCoroutine(speedAndJumpPowerUp(_pl));
            }
        }
    }

    //------- IEnumerator for add force to jump ---------\\
    IEnumerator jumpPowerUp(PlayerMovement _playerScript)
    {
        float _defaultJumpForce = _playerScript.jumpForce;
        _playerScript.jumpForce += _playerScript.jumpForce / 10;
        yield return new WaitForSeconds(_durationOfJumpPowerUp);
        _playerScript.jumpForce = _defaultJumpForce;
        Destroy(this.gameObject);
    }

    //------- IEnumerator for add force to jump ---------\\
    IEnumerator speedPowerUp(PlayerMovement _playerScript)
    {
        float _defaultSpeed = _playerScript.speed;
        _playerScript.speed += _playerScript.speed / 10;
        yield return new WaitForSeconds(_durationOfSpeedPowerUp);
        _playerScript.speed = _defaultSpeed;
        Destroy(this.gameObject);
    }

    //------- IEnumerator for add force speed and jump ---------\\
    IEnumerator speedAndJumpPowerUp(PlayerMovement _playerScript)
    {
        float _defaultJumpForce = _playerScript.jumpForce;
        float _defaultSpeed = _playerScript.speed;
        _playerScript.speed += _playerScript.speed / 10;
        _playerScript.jumpForce += _playerScript.jumpForce / 10;
        yield return new WaitForSeconds(_durationOfSpeedAndJumpPowerUp);
        _playerScript.speed = _defaultSpeed;
        Destroy(this.gameObject);
    }

    //------- Function for set off components ---------\\
    void SetOffComponents()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
