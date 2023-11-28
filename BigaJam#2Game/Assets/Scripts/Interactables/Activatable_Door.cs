using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class Activatable_Door : Activatable_Base
{
    [Tooltip("How fast the door will move in units per second when opened/closed.")]
    [Min(0f)]
    [SerializeField] private float _MoveSpeed = 0.5f;

    [Tooltip("How much of the door is still visible when open. Make sure the bottom of the door is flush with the ground, or this may not appear to work right even though it is.")]
    [Min(0f)]
    [SerializeField] private float _Lip = 0.1f;


    private Transform _Door;
    private BoxCollider2D _DoorCollider;

    private Vector3 _StartPosition;

    private float _DoorClosedPositionY;
    private float _DoorOpenPositionY;



    new void Awake()
    {
        base.Awake();

        _Door = transform.Find("Door");

        if (_Door == null)
        {
            Debug.LogError("Activatable_Door is missing it's child door object (the actual door)!");
            return;
        }

        _DoorCollider = _Door.GetComponent<BoxCollider2D>();
        _StartPosition = _Door.localPosition;

        if (_DoorCollider != null )
        {
            _DoorClosedPositionY = _Door.transform.localPosition.y;

            _DoorOpenPositionY = _DoorClosedPositionY - (_DoorCollider.size.y * _Door.transform.localScale.y - _Lip);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_StartActivated)
            _Door.localPosition = new Vector2(_StartPosition.x, _DoorOpenPositionY);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them, since player has both.
        if (collision.isTrigger)
            return;


        Activate(collision.gameObject);

        HandleCoroutines();
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        // Triggers will detect other triggers if they have a rigidbody. So we will just ignore them, since player has both.
        if (collision.isTrigger)
            return;


        Deactivate(collision.gameObject);

        HandleCoroutines();
    }

    private void HandleCoroutines()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor());
    }

    private IEnumerator AnimateDoor()
    {
        if (_Door == null || _DoorCollider == null)
            yield break;


        Vector2 moveDir = IsActivated ? Vector2.down : Vector3.up;

        float stopPosY = IsActivated ? _DoorOpenPositionY : _DoorClosedPositionY;

        float yPos = -1f;
        while (yPos != stopPosY)
        {
            Vector3 pos = _Door.localPosition;
            yPos = pos.y + (moveDir.y * _MoveSpeed * Time.deltaTime);

            yPos = Mathf.Clamp(yPos, _DoorOpenPositionY, _DoorClosedPositionY);
            _Door.localPosition = new Vector2(_StartPosition.x, yPos);

            yield return null;
        }
    }

    public override void Activate(GameObject sender)
    {
        base.Activate(sender);

        HandleCoroutines();
    }

    public override void Deactivate(GameObject sender)
    {
        base.Deactivate(sender);

        StopAllCoroutines();

        HandleCoroutines();
    }
}
