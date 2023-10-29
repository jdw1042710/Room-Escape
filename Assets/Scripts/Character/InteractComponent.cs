using System;
using UnityEngine;

public class InteractionComponent : MonoBehaviour
{
    [SerializeField] private float range = 5.0f;
    private Interactable _interactObject;

    private void SetInteractObject(Interactable other)
    {
        if (_interactObject is null)
        {
            _interactObject = other;
        }
        else if(_interactObject != other)
        {
            _interactObject.ExitTrigger(gameObject);
            _interactObject = other;
        }
    }
    
    private void FixedUpdate()
    {
        if (CheckInteractableObject(out var hit))
        {
            var other = hit.transform.GetComponent<Interactable>();
            SetInteractObject(other);
            
            if (_interactObject is null) return;
            _interactObject.EnterTrigger(gameObject);
        }
        else
        {
            SetInteractObject(null);
        }
    }

    private void Update()
    {

        if (InputManager.Instance.isInteract && _interactObject != null)
        {
            _interactObject.Action(gameObject);
        }
    }

    bool CheckInteractableObject(out RaycastHit hit)
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Interaction");
        return Physics.Raycast(
            transform.position, 
            transform.forward, 
            out hit, 
            range, 
            layerMask);
    }
}
