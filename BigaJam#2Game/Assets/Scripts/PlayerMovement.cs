using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybuttons")]
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _dashInput;
    [SerializeField] private InputActionReference _jumpInput;
    [Space]
    [Tooltip("This multiplier effects how fast the player's dash uses energy.")]
    public float _dashEnergyCostMultiplier = 3f;


    private Animator _anim;

    public float walkSpeed = 2f;
    public float dashSpeed = 4f;

    public int _playerIndex;

    private Vector2 _MoveVector;

    private Rigidbody2D _rb;
    private PlayerStats _PlayerStats;


    private void Awake()
    {
        _jumpInput.action.performed += Jump;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _PlayerStats = GetComponent<PlayerStats>();
        //_gunScript.SetPlayer(PlayerIndex);
    }
    private void OnDestroy() 
    {
        _jumpInput.action.performed -= Jump;
    }
    void Update()
    {
        _MoveVector.x = _moveInput.action.ReadValue<float>();

        if (_MoveVector.x != 0f && !_dashInput.action.IsPressed())
            Walk();
        else if (_dashInput.action.IsPressed())
            Dash();
        else
            Stop();


        Reflect();
        CheckingGround();
    }

    //------- Function for first player walk ---------

    private Vector2 moveVectorFirst;

    void Walk()
    {
        Move(walkSpeed);
    }

    void Dash()
    {
        if (_PlayerStats.Energy <= 0)
            return;


        Move(dashSpeed);

        // Remove speed units of energy per second.
        _PlayerStats.RemoveEnergy(_dashEnergyCostMultiplier * dashSpeed * Time.deltaTime);
    }

    private void Stop()
    {
        _anim.SetFloat("moveX", Mathf.Abs(_MoveVector.x));
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
        _MoveVector = Vector2.zero;
    }

    private void Move(float speed)
    {
        _anim.SetFloat("moveX", Mathf.Abs(_MoveVector.x));
        _rb.velocity = new Vector2(_MoveVector.x * speed, _rb.velocity.y);
    }

    public bool faceRight = true;

    void Reflect()
    {
        if ((_MoveVector.x > 0 && !faceRight) || (_MoveVector.x < 0 && faceRight))
        {
            if (faceRight)
            {
                GetComponent<SpriteRenderer>().flipX = true; 
            }

            if (!faceRight) {
                GetComponent<SpriteRenderer>().flipX = false;
            } 

            faceRight = !faceRight;
        }
    }

    //------- Function for jump ---------

    public float jumpForce = 6;

    void Jump(InputAction.CallbackContext ctx)
    {
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

        _anim.SetBool("onGround", onGround);
    }
    //-----------------------------------------------------------------

    //------- Function for take players damage ---------
    public void TakeDamage()
    {
        StartCoroutine(TakeDamageTimer());
        GetComponent<PlayerStats>().RemoveHP(10f);
    }

    IEnumerator TakeDamageTimer()
    {
        _anim.SetBool("Damage", true);
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("Damage", false);
    }
    //-----------------------------------------------------------------

    public void Repulsion(float power)
    {
        //if (faceRight) _rb.AddForce(transform.right * power, ForceMode2D.Impulse); 
        //if (!faceRight) _rb.AddForce(-transform.right * power, ForceMode2D.Impulse);
    }
}
