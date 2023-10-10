using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] GameObject playerGO;

    [Header("EnemySO reference")]
    [SerializeField] EnemySO enemy;

    private bool isPlayerInSightRange, isPlayerInAttackRange;

    void Start()
    {
        
    }

    void Update()
    {
        SearchForPlayer();
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange);


    }

}
