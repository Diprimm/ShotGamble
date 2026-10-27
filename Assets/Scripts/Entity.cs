using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private bool dungeonEnemy;
    public ParticleSystem dieParticles;
    public GameObject dieLoot;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dungeonEnemy)
        {
            LevelManager.instance.EnemyDied();
        }
        if (dieParticles != null)
        {
            Instantiate(dieParticles, transform.position, Quaternion.identity);
        }
        if (dieLoot != null)
        {
            Instantiate(dieLoot, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
