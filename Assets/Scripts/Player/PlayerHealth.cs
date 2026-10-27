using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnHealthChange;

    public int maxHealth;
    public int currentHealth;
    public ParticleSystem dieParticles;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            OnHealthChange?.Invoke();
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            OnHealthChange?.Invoke();
        }
    }



    private void Die()
    {
        if (dieParticles != null)
        {
            Instantiate(dieParticles, transform.position, Quaternion.identity);
        }
    }
}
