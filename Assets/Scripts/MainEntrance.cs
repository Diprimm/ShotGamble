using UnityEngine;

public class MainEntrance : MonoBehaviour
{
    public bool canUseEntrance;
    public GameObject sunLight;
    [SerializeField] private CharacterController playerCC;

    [SerializeField] private bool canTeleport => playerCC != null;

    public Transform destination;
   
    
    void Update()
    {
        if (canTeleport && Input.GetKeyDown(KeyCode.E) && canUseEntrance)
        {
            Enter();
        }
    }

    private void Enter()
    {            
        playerCC.enabled = false;
        playerCC.transform.position = destination.transform.position;
        playerCC.transform.rotation = destination.transform.rotation;
        playerCC.enabled = true;
        playerCC = null;

        if (sunLight.activeInHierarchy)
        {
            sunLight.SetActive(false);
        }

        else
        {
            sunLight.SetActive(true);
        }
            Debug.Log("Teleported to: " + destination.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCC = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCC = null;
        }
    }
}
