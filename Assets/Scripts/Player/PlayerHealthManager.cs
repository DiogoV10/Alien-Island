using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IEntity
{
    [Header("Player Health")]
    [SerializeField] public float playerHealth;

    public void Die()
    {
        Debug.Log("PlayerHasDied");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        Time.timeScale = 0f;
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0f) Die();
    }
}
