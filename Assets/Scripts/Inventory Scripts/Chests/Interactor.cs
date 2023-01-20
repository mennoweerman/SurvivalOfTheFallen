using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionPointRadius = 1f;
    public PlayerCam _Playercam;
    public bool isInteracting { get; private set; }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
           
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();
                _Playercam.inMenu = true;
                if (interactable != null) StartInteraction(interactable);
            }
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessfull);
        isInteracting = true;
        
    }

    void EndInteraction()
    {
        isInteracting = false;
       
    }
}
