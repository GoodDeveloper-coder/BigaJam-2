using System.Collections;

public interface IPowerUp
{
    public float Duration { get; set; }
    public IEnumerator ActivatePowerUp(PlayerMovement playerScript);
}