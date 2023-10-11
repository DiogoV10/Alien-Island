using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerCombat playerCombatRef;

    [Header("EnemySO reference")]
    [SerializeField] EnemySO enemy;

    [Header("Enemy")]
    Rigidbody rigidBody;
    //private Vector3 wayPoint = new Vector3();

    private bool isPlayerInSightRange, isPlayerInAttackRange, alreadyAttacked;

    void Start()
    {
        
    }

    void Update()
    {
        SearchForPlayer();
        FollowPLayer();
        if(isPlayerInAttackRange) AttackPlayer();
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, ~3);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, ~3);
        Debug.Log(isPlayerInAttackRange);
        Debug.Log(isPlayerInSightRange);
    }

    void FollowPLayer()
    {
        if (isPlayerInSightRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.speed * Time.deltaTime);
            Debug.Log("Is in Sight Range");
        }
        if (isPlayerInAttackRange) Debug.Log("is in attack range");
    }

    void AttackPlayer()
    {
        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            playerCombatRef.playerHealth -= enemy.damageAmount;
            Debug.Log(playerCombatRef.playerHealth);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemy.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;
}
