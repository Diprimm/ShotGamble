using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isLocked;
    private BoxCollider boxCollider;

    private void Start()
    {
        doorAnimator = GetComponentInParent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        if (isOpen)
        {
            doorAnimator.SetTrigger("open");
            boxCollider.isTrigger = true;
        }

        else
        {
            boxCollider.isTrigger = false;
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);

        if (isLocked)
        {
            if (PlayerInventory.instance.GetPlayerKeys() > 0)
            {
                isLocked = false;
                PlayerInventory.instance.ConsumeKey();
            }

            return;
        }

        if (isOpen)
        {
            doorAnimator.SetTrigger("close");
            isOpen = false;
            boxCollider.isTrigger = false;
        }

        else
        {
            doorAnimator.SetTrigger("open");
            isOpen = true;
            boxCollider.isTrigger = true;
        }
        
    }
}
