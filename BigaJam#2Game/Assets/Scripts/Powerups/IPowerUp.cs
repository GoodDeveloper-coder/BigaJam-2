using System;
using System.Collections;

public interface IPowerUp
{
    public float Duration { get; set; }
    public IEnumerator ActivatePowerUp(PlayerMovement playerScript);
    public event Action OnPickedUp;
}