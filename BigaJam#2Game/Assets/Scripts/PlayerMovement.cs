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
    [SerializeField] private InputActionReference _pauseInput;
    [Space]
    [Tooltip("This multiplier effects how fast the player's dash uses energy.")]
    public float _dashEnergyCostMultiplier = 3f;
    [Tooltip("How quickly the player decellerates.")]
    public float _decellerationRate = 2f;

    [SerializeField] private AudioSource _PickupPowerUp;
    [SerializeField] private AudioSource _JumpSound;


    [Header("Sounds")]
    private Animator _anim;

    public float walkSpeed = 2f;
    public float dashSpeed = 4f;

    public int _playerIndex;

    private Vector2 _MoveVector;

    private Rigidbody2D _rb;
    private PlayerStats _PlayerStats;
    private GunScript _Gun;

    private Vector2 _StartPoint;
    private Activatable_CheckPoint _LastCheckpoint;

    private Dialog_PauseMenu _PauseMenu;


    void Awake()
    {
        _StartPoint = transform.position;

        _PauseMenu = GameObject.FindObjectOfType<Dialog_PauseMenu>(true);
        if (_PauseMenu == null)
            Debug.LogError("There is no pause menu added in this scene!");


        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _PlayerStats = GetComponent<PlayerStats>();
        _Gun = GetComponentInChildren<GunScript>(true);

        _jumpInput.action.performed += Jump;
        _pauseInput.action.performed += Pause;
    }
    void Start()
    {
        //_Gun.SetPlayer(PlayerIndex);
    }
    private void OnDestroy()
    {
        _jumpInput.action.performed -= Jump;
        _pauseInput.action.performed -= Pause;
    }
    void Update()
    {
        _MoveVector.x = _moveInput.action.ReadValue<float>();

        if (_MoveVector.x != 0f && !_dashInput.action.IsPressed())
            Move(walkSpeed);
        else if (_MoveVector.x != 0f && _dashInput.action.IsPressed())
            Dash();
        else
            Stop();


        Reflect();
        CheckingGround();
    }

    //------- Function for first player walk ---------

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
        // Velocity needs to be less than this value for the walk animation to stop.
        float animThreshold = 0.25f;

        // Using rb.velocity here makes player still animate during knockback.
        float moveAmount = _rb.velocity.x;
        if (moveAmount < animThreshold && moveAmount > -animThreshold)
            moveAmount = 0f;
        _anim.SetFloat("moveX", Mathf.Abs(moveAmount));

        float decelAmount = _decellerationRate * Time.deltaTime;
        if (_rb.velocity.x > 0f)
            decelAmount = -decelAmount;

        _rb.velocity = new Vector2(_rb.velocity.x + decelAmount, _rb.velocity.y);
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

    void Pause(InputAction.CallbackContext ctx)
    {
        // If menu is already open, then do nothing.
        if (_PauseMenu.gameObject.activeSelf)
            return;

        _PauseMenu.OpenDialog();
    }

    //------- Function for jump ---------

    public float jumpForce = 6;

    void Jump(InputAction.CallbackContext ctx)
    {
        if (onGround)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _JumpSound.Play();
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
    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageTimer());
        GetComponent<PlayerStats>().RemoveHP(damage);
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

    public void SetCheckPoint(Activatable_CheckPoint checkPoint)
    {
        _LastCheckpoint = checkPoint;
    }

    public void WarpToLastCheckPoint()
    {
        if (_LastCheckpoint != null)
            transform.position = _LastCheckpoint.transform.position;
        else
            transform.position = _StartPoint;
    }

    public void PickupPowerupSound()
    {
        _PickupPowerUp.Play();
    }

    public PlayerStats PlayerStats { get { return _PlayerStats; } }
    public GunScript Gun { get { return _Gun; } }

}
