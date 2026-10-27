using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private AmmoType type;
    [SerializeField] private int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AmmoManager manager))
        {           
            manager.AddAmmo(type, amount);
            Destroy(gameObject);
        }
    }
}
