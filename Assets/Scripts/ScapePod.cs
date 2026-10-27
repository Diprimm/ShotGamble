using UnityEngine;

public class ScapePod : MonoBehaviour
{
    public int catsCollected = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WeaponPickUp catPickUp) && other.CompareTag("Cat"))
        {
            catsCollected++;
            LevelManager.instance.RescueCat();
            Debug.Log("Cat collected");
            catPickUp.Despawn();
        }
    }
}
