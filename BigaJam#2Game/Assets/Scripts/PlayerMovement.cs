using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _jumpInput;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_gunScript.SetPlayer(PlayerIndex);
    }
    private void Awake() 
    {
        _jumpInput.action.performed += Jump;
    }
    private void OnDestroy() 
    {
        _jumpInput.action.performed -= Jump;
    }
    void Update()
    {
        Walk();
        Reflect();
    }

    //------- Function for first player walk ---------

    private Vector2 moveVectorFirst;

    void Walk()
    {
        moveVectorFirst.x = _moveInput.action.ReadValue<float>();
        _rb.velocity = new Vector2(moveVectorFirst.x * speed, _rb.velocity.y);
    }

    public bool faceRight = true;

    void Reflect()
    {
        if ((moveVectorFirst.x > 0 && !faceRight) || (moveVectorFirst.x < 0 && faceRight))
        {
            if (faceRight) GetComponent<SpriteRenderer>().flipX = true;
            if (!faceRight) GetComponent<SpriteRenderer>().flipX = false;
            //transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    //------- Function for jump ---------

    public int jumpForce = 6;

    void Jump(InputAction.CallbackContext ctx)
    {
        CheckingGround();
        if (onGround)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
    }

    //------- Function for detecting ground ---------

    public bool onGround;
    public LayerMask Ground;
    public Transform GroundCheck;
    private float GroundCheckRadius = 0.08980194f;

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
    }
    //-----------------------------------------------------------------
}
