using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IEntity
{


    public event Action<float> OnHealthChanged;


    [Header("Player Health")]
    [SerializeField] private float playerHealthMax;


    [SerializeField] private float playerHealth;


    private void Awake()
    {
        IncrementCurrentHealth(playerHealthMax);

        PlayerUpgrades.Instance.OnUpgradeUnlocked += PlayerUpgrades_OnUpgradeUnlocked;
    }

    private void PlayerUpgrades_OnUpgradeUnlocked(object sender, PlayerUpgrades.OnUpgradeUnlockedEventArgs e)
    {
        switch (e.upgradeType)
        {
            case PlayerUpgrades.UpgradeType.HealthMax_1:
                SetMaxHealth(150f);
                break;
            case PlayerUpgrades.UpgradeType.HealthMax_2:
                SetMaxHealth(200f);
                break;
            case PlayerUpgrades.UpgradeType.HealthMax_3:
                SetMaxHealth(300f);
                break;
            default:
                break;
        }
    }

    private void SetMaxHealth(float health)
    {
        float previousMaxHealth = playerHealthMax;
        playerHealthMax = health;
        IncrementCurrentHealth(playerHealthMax - previousMaxHealth);
    }

    public void Die()
    {
        Debug.Log("PlayerHasDied");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        Time.timeScale = 0.2f;
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        OnHealthChanged?.Invoke(playerHealth);
        if (playerHealth <= 0f) Die();
    }

    public void IncrementCurrentHealth(float health)
    {
        if (playerHealth < playerHealthMax)
            playerHealth += health;

        if (playerHealth > playerHealthMax)
            playerHealth = playerHealthMax;

        OnHealthChanged?.Invoke(playerHealth);
    }

    public float GetHealth()
    {
        return playerHealth;
    }
    
    public float GetMaxHealth()
    {
        return playerHealthMax;
    }

    
}
