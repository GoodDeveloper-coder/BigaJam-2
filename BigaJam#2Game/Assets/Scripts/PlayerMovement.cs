using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybuttons")]
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _jumpInput;
    [Space]

    [SerializeField]private GameObject _swapGun;

    private Animator _anim;

    public float speed = 2f;

    public int _playerIndex;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
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
        CheckingGround();
    }

    //------- Function for first player walk ---------

    private Vector2 moveVectorFirst;

    void Walk()
    {
        moveVectorFirst.x = _moveInput.action.ReadValue<float>();
        _anim.SetFloat("moveX", Mathf.Abs(moveVectorFirst.x));
        _rb.velocity = new Vector2(moveVectorFirst.x * speed, _rb.velocity.y);
    }

    public bool faceRight = true;

    void Reflect()
    {
        if ((moveVectorFirst.x > 0 && !faceRight) || (moveVectorFirst.x < 0 && faceRight))
        {
            if (faceRight)
            {
                GetComponent<SpriteRenderer>().flipX = true; 
                _swapGun.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (!faceRight) {
                GetComponent<SpriteRenderer>().flipX = false;
                _swapGun.transform.localScale = new Vector3(1f, 1f, 1f);
            } 

            //transform.localScale *= new Vector2(-1, 1);
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
        if (faceRight) _rb.AddForce(transform.right * power, ForceMode2D.Impulse); 
        if (!faceRight) _rb.AddForce(-transform.right * power, ForceMode2D.Impulse);
    }
}
