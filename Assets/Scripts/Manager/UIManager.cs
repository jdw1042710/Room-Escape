using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HUDController HUD;

    public static UIManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowTriggerMessage(string message)
    {
        if (!HUD) return;
        HUD.ShowTriggerMessage(message);
    }
    
    public void HideTriggerMessage()
    {
        if (!HUD) return;
        HUD.HideTriggerMessage();
    }
    
    public void ShowInteractionMessage(string message)
    {
        if (!HUD) return;
        HUD.ShowInteractionMessage(message);
    }
    
    public void HideInteractionMessage()
    {
        if (!HUD) return;
        HUD.HideInteractionMessage();
    }
    
    
}
