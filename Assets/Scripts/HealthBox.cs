using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public int healthRestoreAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            if (other.GetComponent<PlayerHealth>().currentHealth < other.GetComponent<PlayerHealth>().maxHealth)
            {
                other.GetComponent<PlayerHealth>().Heal(healthRestoreAmount);
                Destroy(gameObject);
            }
        }
    }
}
