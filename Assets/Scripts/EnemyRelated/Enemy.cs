using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("EnemySO reference")]
    [SerializeField] EnemySO enemy;

    private bool isPlayerInSightRange, isPlayerInAttackRange, alreadyAttacked;

    void Update()
    {
        SearchForPlayer();
        FollowPLayer();
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, whatIsPlayer);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, whatIsPlayer);
    }

    void FollowPLayer()
    {
        if (isPlayerInSightRange && !isPlayerInAttackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.speed * Time.deltaTime);
        }
        if (isPlayerInAttackRange) AttackPlayer();
    }

    void AttackPlayer()
    {
        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            IEntity entity = player.GetComponent<IEntity>();
            entity.TakeDamage(enemy.damageAmount);
            Debug.Log("Attacked!");
            Debug.Log(playerHealthManager.playerHealth);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemy.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public void TakeDamage(float damage)
    {
        enemy.enemyHP -= damage;
        Debug.Log(enemy.enemyHP);
        if (enemy.enemyHP <= 0f) Die();
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        Destroy(gameObject);
    }
}
