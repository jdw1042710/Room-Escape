using UnityEngine;

public interface Interactable
{
    public void Action(GameObject other);
    public void EnterTrigger(GameObject other);
    public void ExitTrigger(GameObject other);
}
