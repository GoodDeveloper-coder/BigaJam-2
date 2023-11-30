using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// An interactable lift.
/// </summary>
/// <remarks>
/// To set the start and end positions of the lift, move the Start Point and End Point child objects to the
/// desired positions. This script shows you a ghost image of the lift at both positions in the editor
/// so you can see if it is positioned well or not. That will make it much easier to avoid having it overlapping
/// a wall for example and position it just where you want it. Lastly, this script also draws a yellow line
/// in the editor between the start and end points to visualize the lift's path.
///
/// To change the size of the lift, set the scale property of the "Lift" child object, which is the visible representation of the lift.
/// 
/// If StartActive is enabled, the lift will start at its end position. If the MoveBackAndForthForeverWhileActivated
/// option is also on, then the lift will stop when it is activated.
/// </remarks>
[Serializable]
public class Activatable_Lift : Activatable_Base
{
    [Header("Lift Settings")]

    [Tooltip("How fast the door will move in units per second when opened/closed.")]
    [Min(0f)]
    [SerializeField] private float _MoveSpeed = 0.5f;

    [Tooltip("If this option is on, then the lift will keep moving back and forth as long as it is being activated. Otherwise it will just stop when it reaches its end point, and move back when deactivated.")]
    [SerializeField] private bool _MoveBackAndForthForeverWhileActivated = false;

    [SerializeField] private GameObject _StartPoint;
    [SerializeField] private GameObject _EndPoint;


    private Transform _Lift;
    private BoxCollider2D _LiftCollider;

    private Vector2 _StartPosition;
    private Vector2 _EndPosition;

    private Vector2 _LiftStartPosition;
    private Vector2 _LiftEndPosition;

    private float _TotalDistance; // Holds the distance between the start and end points.
    private float _PercentMoved; // Holds the percentage of the distance that the lift is from the start point to the end point.

    private Vector2 _MoveDirection;



    new void Awake()
    {
        base.Awake();

        _Lift = transform.Find("Lift");

        if (_Lift == null)
        {
            Debug.LogError($"Activatable_Lift \"{gameObject.name}\" is missing it's child lift object (the actual door)!");
            return;
        }

        if (_StartPoint == null || _EndPoint == null)
            Debug.LogError($"Activatable_Lift \"{gameObject.name}\" does not have either the start or end point set!");


        _LiftCollider = _Lift.GetComponent<BoxCollider2D>();


        // Disable the sprite renderers for the ghosts lifts at the start/end points so they aren't there in play mode.
        _StartPoint.transform.Find("Ghost").GetComponent<SpriteRenderer>().enabled = false;
        _EndPoint.transform.Find("Ghost").GetComponent<SpriteRenderer>().enabled = false;

        // Get the lift's start and end positions.
        _StartPosition = _StartPoint.transform.localPosition;
        _EndPosition = _EndPoint.transform.localPosition;


        _MoveDirection = (_EndPosition - _StartPosition).normalized;
        _TotalDistance = Vector2.Distance(_StartPosition, _EndPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_StartActivated)
        {
            _Lift.localPosition = _StartPosition;
        }
        else
        {
            _Lift.localPosition = _EndPosition;
        }


        HandleCoroutines();
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
        StartCoroutine(AnimateLift());
    }

    private IEnumerator AnimateLift()
    {
        if (_Lift == null)
        {
            Debug.LogError("NULL");
            yield break;
        }


        bool moveForward = IsActivated;

        Vector2 pos = Vector2.zero;
        while (true)
        {
            pos = _Lift.localPosition;

            float distanceFromStart = Vector2.Distance(_StartPosition, pos);

            float moveDist = _MoveSpeed * Time.deltaTime;
            if (!moveForward)
                moveDist *= -1;
            //Debug.Log("Move: " + moveDist);
            distanceFromStart = Mathf.Clamp(distanceFromStart + moveDist, 0f, _TotalDistance);

            float percentMoved = distanceFromStart / _TotalDistance;
            percentMoved = Mathf.Clamp(percentMoved, 0f, 1f);

            pos = _StartPosition + Vector2.Lerp(_StartPosition, _EndPosition, percentMoved);

            _Lift.localPosition = pos;



            if (_MoveBackAndForthForeverWhileActivated && IsActivated) 
                //((!_StartActivated && moveForward) || (_StartActivated && !moveForward)))
            {
                //Debug.Log(percentMoved);
                if (percentMoved == 0f || percentMoved == 1f)
                    moveForward = !moveForward;
            }
            else
            {
                if (percentMoved == 0f || percentMoved == 1f)
                    break;
            }

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if (_StartPoint == null || _EndPoint == null)
            return;


        if (_Lift == null)
            _Lift = transform.Find("Lift");


        // Make sure the ghost lifts showing the start and end positions are the same size as the actual lift.
        _StartPoint.transform.localScale = _Lift.transform.localScale;
        _EndPoint.transform.localScale = _Lift.transform.localScale;

        // Draw a line between the lift's start and end points.
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_StartPoint.transform.position, _EndPoint.transform.position);
    }

    public override void Activate(GameObject sender)
    {
        base.Activate(sender);

        HandleCoroutines();
    }

    public override void Deactivate(GameObject sender)
    {
        base.Deactivate(sender);

        HandleCoroutines();
    }
}
