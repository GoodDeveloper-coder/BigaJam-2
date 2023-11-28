using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class Activatable_Base : MonoBehaviour, IActivatable
{
    [Tooltip("Any targets specified in this list will be activated/deactivated whenever this object is activated.")]
    [SerializeField] public List<Activatable_Base> _Targets;

    [Tooltip("Whether or not this object should start already activated. If so, it will deactivate when targeted rather than activate.")]
    [SerializeField] public bool _StartActivated = false;

    [Tooltip("If this option is enabled, then this object will stay on forever once it has been activated.")]
    [SerializeField] public bool _StayActivatedForever = false;



    /// <summary>
    /// This keeps track of all objects that are currently powering this one.
    /// </summary>
    protected List<GameObject> _ActivationSources = new List<GameObject>();



    protected void Awake()
    {
        IsActivated = _StartActivated;

        // Remove any duplicate targets specified in the inspector.
        RemoveDuplicateTargets();
    }

    public virtual void Activate(GameObject sender)
    {
        AddActivationSource(sender);

        if (IsActivated == !_StartActivated)
            return;

        IsActivated = !_StartActivated;
        

        // Activate all specified targets.
        for (int i = 0; i < _Targets.Count; i++)
        {
            IActivatable target = _Targets[i];

            target.Activate(gameObject);
        }
    }

    public virtual void Deactivate(GameObject sender)
    {
        if (IsActivated == _StartActivated)
            return;


        RemoveActivationSource(sender);

        if (CurrentActivationsCount > 0 || StayActivatedForever)
            return;


        IsActivated = _StartActivated;


        // Deactivate all specified targets.
        for (int i = 0; i < _Targets.Count; i++)
        {
            IActivatable target = _Targets[i];

            target.Deactivate(gameObject);
        }

    }

    protected void AddActivationSource(GameObject source)
    {
        if (_ActivationSources.Contains(source))
            return;

        _ActivationSources.Add(source);
    }

    protected void RemoveActivationSource(GameObject source)
    {
        if (_ActivationSources.Contains(source))
            _ActivationSources.Remove(source);
        else
            Debug.LogWarning($"Object \"{gameObject.name}\" somehow got deactivated by an object that did not previously activate it.", gameObject);
    }

    protected void RemoveDuplicateTargets()
    {
        List<Activatable_Base> noDuplicates = new List<Activatable_Base>();

        foreach (Activatable_Base target in _Targets)
        {
            if (!noDuplicates.Contains(target))
                noDuplicates.Add(target);

            if (target.IsTargeting(this))
                Debug.LogWarning($"Activatable \"{gameObject.name}\" is targeting \"{target.gameObject.name}\", which in turn is targeting \"{gameObject.name}\". Was this intentional? This connection loop will cause the first object to stay activated regardless of its StayActivatedForever setting.", gameObject);
        }

        _Targets = noDuplicates;
    }

    protected bool SourceContainsTarget(GameObject activationSource, IActivatable targetToCheckFor)
    {
        IActivatable activatableSource = activationSource.GetComponent<IActivatable>();

        if (activatableSource == null)
            return false;

        return activatableSource.IsTargeting(targetToCheckFor);
    }

    public bool IsTargeting(IActivatable targetToCheck)
    {
        foreach (IActivatable curTarget in _Targets)
        {
            if (curTarget.gameObject == targetToCheck.gameObject)
                return true;
        }

        return false;
    }

    public bool IsActivated { get; private set; }

    public bool StayActivatedForever
    {
        get { return _StayActivatedForever; }
        protected set { _StayActivatedForever = value; }
    }


    /// <summary>
    /// This tracks how many objects are currently interacting with this one. For example, this way if two things are on a button it won't deactivate until both move off of it.
    /// </summary>
    public int CurrentActivationsCount { get { return _ActivationSources.Count; } }

}
