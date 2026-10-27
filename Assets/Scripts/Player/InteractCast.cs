using UnityEngine;

public class InteractCast : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactable;
    
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance, interactable))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}
