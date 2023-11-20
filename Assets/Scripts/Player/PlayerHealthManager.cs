using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IEntity
{
    [Header("Player Health")]
    [SerializeField] public float playerHealth;
    public event Action<float> OnHealthChanged;

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
        OnHealthChanged?.Invoke(playerHealth);
        if (playerHealth <= 0f) Die();
    }

    public void GetLife()
    {
        if(playerHealth < 100) playerHealth += 15;
        OnHealthChanged?.Invoke(playerHealth);
    }
}
