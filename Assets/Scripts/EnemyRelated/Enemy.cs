using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] GameObject player;

    [Header("EnemySO reference")]
    [SerializeField] EnemySO enemy;

    [Header("Enemy")]
    Rigidbody rigidBody;
    //private Vector3 wayPoint = new Vector3();

    private bool isPlayerInSightRange, isPlayerInAttackRange;

    void Start()
    {
        
    }

    void Update()
    {
        SearchForPlayer();
        FollowPLayer();
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, 3);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, 3);
    }

    void FollowPLayer()
    {
        if (isPlayerInSightRange) 
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.speed * Time.deltaTime);
        }
    }

    void AttackPlayer()
    {

    }
}
