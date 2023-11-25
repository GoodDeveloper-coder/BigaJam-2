using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] int playerIndex;

    public float speed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        if (playerIndex == 2) 
        {
            WalkSecondPlayer();
        }

        if (playerIndex == 1)
        {
            WalkFirstPlayer();
        }

        Jump();
        Reflect();
        CheckingGround();
    }

    //------- Function for first player walk ---------

    private Vector2 moveVectorFirst;

    void WalkFirstPlayer()
    {
        moveVectorFirst.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(1 * speed, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
        }
    }

    //------- Function for second player walk ---------

    private Vector2 moveVectorSecond;

    void WalkSecondPlayer()
    {
        moveVectorSecond.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(1 * speed, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
        }
    }

    //------- Function for rotating charavter on Horizontal axe ---------

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
        if (playerIndex == 1)
        {
            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else if (playerIndex == 2)
        {
            if (onGround && Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
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

