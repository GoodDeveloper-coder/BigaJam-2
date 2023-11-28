using UnityEngine;


public interface IActivatable
{
    void Activate(GameObject source);
    void Deactivate(GameObject source);
    bool IsTargeting(IActivatable target);



    GameObject gameObject { get; }

    bool IsActivated { get; }
}
