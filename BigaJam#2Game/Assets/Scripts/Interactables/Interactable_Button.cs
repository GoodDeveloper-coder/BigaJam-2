using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Interactable_Button : Interactable_Base
{
    [Tooltip("How fast the button will move in units per second when pressed/released.")]
    [Min(0f)]
    [SerializeField] protected float _MoveSpeed = 0.5f;

    [Tooltip("How much of the button is still visible when pressed.")]
    [Min(0f)]
    [SerializeField] private float _Lip = 0.05f;


    private Transform _Button;
    private BoxCollider2D _ButtonCollider;

    private Vector3 _StartPosition;

    private float _ButtonUnpressedPositionY;
    private float _ButtonPressedPositionY;



    new void Awake()
    {
        base.Awake();

        _Button = transform.Find("PushButton");

        if (_Button == null)
        {
            Debug.LogError("Interactable_Button is missing it's child button object (the actual button)!");
            return;
        }

        _ButtonCollider = _Button.GetComponent<BoxCollider2D>();

        _StartPosition = _Button.localPosition;

        if (_ButtonCollider != null)
        {
            _ButtonUnpressedPositionY = _Button.transform.localPosition.y;
            _ButtonPressedPositionY = _ButtonUnpressedPositionY - (_ButtonCollider.size.y * _Button.transform.localScale.y - _Lip);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them since player has both.
        if (collision.isTrigger)
            return;


        BeginInteraction(collision.gameObject);

        HandleCoroutines();
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them since player has both.
        if (collision.isTrigger)
            return;


        EndInteraction(collision.gameObject);

        HandleCoroutines();
    }

    private void HandleCoroutines()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        if (_Button == null || _ButtonCollider == null)
            yield break;


        Vector2 moveDir = IsActivated ? Vector2.down : Vector3.up;

        float stopPosY = IsActivated ? _ButtonPressedPositionY : _ButtonUnpressedPositionY;

        float yPos = -1f;
        while (yPos != stopPosY)
        {
            Vector3 pos = _Button.localPosition;
            yPos = pos.y + (moveDir.y * _MoveSpeed * Time.deltaTime);

            yPos = Mathf.Clamp(yPos, _ButtonPressedPositionY, _ButtonUnpressedPositionY);
            _Button.localPosition = new Vector2(pos.x, yPos);

            yield return null;
        }

    }


}
