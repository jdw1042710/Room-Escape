using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI triggerMessage;
    [SerializeField] private TextMeshProUGUI interactionMessage;

    public void ShowTriggerMessage(string message)
    {
        triggerMessage.text = message;
        triggerMessage.gameObject.SetActive(true);
    }

    public void HideTriggerMessage()
    {
        triggerMessage.gameObject.SetActive(false);
    }
    
    public void ShowInteractionMessage(string message)
    {
        interactionMessage.text = message;
        interactionMessage.gameObject.SetActive(true);
    }
    
    public void HideInteractionMessage()
    {
        interactionMessage.gameObject.SetActive(false);
    }
}
