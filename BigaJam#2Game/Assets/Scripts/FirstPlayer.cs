using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : MonoBehaviour
{

    [SerializeField] private float speed = 2f;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Walk();
        Reflect();
        Jump();
        CheckingGround();
    }

    //------- Function for second player walk ---------

    private Vector2 moveVectorFirst;

    void Walk()
    {
        moveVectorFirst.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.D))
        {
            _rb.velocity = new Vector2(1 * speed, _rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rb.velocity = new Vector2(-1 * speed, _rb.velocity.y);
        }
    }

    public bool faceRight = true;

    void Reflect()
    {
        if ((moveVectorFirst.x > 0 && !faceRight) || (moveVectorFirst.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    //------- Function for jump ---------

    public int jumpForce = 6;

    void Jump()
    {
        if (onGround && Input.GetKeyDown(KeyCode.Space))
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
