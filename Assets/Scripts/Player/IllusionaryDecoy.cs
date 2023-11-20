using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionaryDecoy : MonoBehaviour, IEntity
{


    [Header("Detection Parameter")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Stats")]
    [SerializeField] private float health;

    private SkillSO skill;

    private float startTime;


    private void Start()
    {
        startTime = Time.time;

        NotifyNearbyEnemies();
    }

    private void Update()
    {
        if (Time.time - startTime >= skill.duration)
        {
            SetEnemiesToTargetPlayer();
            Destroy(gameObject);
        }
        else
        { 
            NotifyNearbyEnemies();
        }

    }

    public void Initialize(SkillSO skillSO)
    {
        skill = skillSO;
    }

    private void NotifyNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, skill.range, enemyLayer);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetTarget(gameObject);
            }
        }
    }

    private void SetEnemiesToTargetPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, skill.range, enemyLayer);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetTargetToPlayer();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        SetEnemiesToTargetPlayer();
        Destroy(gameObject);
    }
}
