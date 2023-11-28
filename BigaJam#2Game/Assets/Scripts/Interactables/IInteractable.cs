using UnityEngine;


public interface IInteractable
{
    void BeginInteraction(GameObject source);
    void EndInteraction(GameObject source);



    int ActiveInteractionsCount { get; }
    bool IsBeingInteractedWith { get; }
}
