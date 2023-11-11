using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    public enum DamageType
    {
        Small,
        Medium,
        Big,
    }

    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("EnemySO reference")]
    [SerializeField] EnemySO enemy;

    [Header("Enemy States")]
    [SerializeField] private WanderState wanderState;
    [SerializeField] private SearchState searchState;
    [SerializeField] private ChaseState chaseState;
    [SerializeField] private AttackState attackState;
    [SerializeField] private HitState hitState;
    [SerializeField] private DeadState deadState;

    private Animator animator;
    private Rigidbody rb;

    private float lastSightTime;
    private float timeSinceLastSight;

    private float health;

    private bool isPlayerInSightRange, isPlayerInAttackRange, alreadyAttacked;
    private bool canSeePlayer;

    private IEnemyState currentState;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        deadState.Initialize(animator);

        rb.isKinematic = false;
    }

    private void Start()
    {
        wanderState.gameObject.SetActive(true);
        attackState.gameObject.SetActive(false);
        chaseState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        hitState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        currentState = wanderState;
        currentState.EnterState(this);

        health = enemy.enemyHP;
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void ChangeState(IEnemyState newState)
    {
        currentState.ExitState(this);

        currentState = newState;
        currentState.EnterState(this);
    }

    public void TransitionToSearch()
    {
        searchState.gameObject.SetActive(true);
        wanderState.gameObject.SetActive(false);
        attackState.gameObject.SetActive(false);
        chaseState.gameObject.SetActive(false);
        hitState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        ChangeState(searchState);
    }
    
    public void TransitionToWander()
    {
        wanderState.gameObject.SetActive(true);
        attackState.gameObject.SetActive(false);
        chaseState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        hitState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        ChangeState(wanderState);
    }
    
    public void TransitionToAttack()
    {
        attackState.gameObject.SetActive(true);
        chaseState.gameObject.SetActive(false);
        wanderState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        hitState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        ChangeState(attackState);
    }

    public void TransitionToChase()
    {
        chaseState.gameObject.SetActive(true);
        attackState.gameObject.SetActive(false);
        wanderState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        hitState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        ChangeState(chaseState);
    }

    public void TransitionToHit()
    {
        hitState.gameObject.SetActive(true);
        chaseState.gameObject.SetActive(false);
        attackState.gameObject.SetActive(false);
        wanderState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        deadState.gameObject.SetActive(false);
        ChangeState(hitState);
    }
    
    public void TransitionToDead()
    {
        deadState.gameObject.SetActive(true);
        hitState.gameObject.SetActive(false);
        chaseState.gameObject.SetActive(false);
        attackState.gameObject.SetActive(false);
        wanderState.gameObject.SetActive(false);
        searchState.gameObject.SetActive(false);
        ChangeState(deadState);
    }

    public void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, enemy.speed * Time.deltaTime);
        transform.LookAt(destination);
    }

    public Vector3 GetPlayerPosition()
    {
        if (player != null)
        {
            return player.transform.position;
        }
        return Vector3.zero;
    }

    public bool SearchForPlayer()
    {
        if (player != null)
        {
            
            isPlayerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, whatIsPlayer);

            if (isPlayerInSightRange)
            {
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, enemy.sightRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        lastSightTime = Time.time;
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }

        timeSinceLastSight = Time.time - lastSightTime;

        return canSeePlayer;
    }

    public bool IsPlayerInAttackRange ()
    {
        return isPlayerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, whatIsPlayer);
    }

    public float TimeSinceLastSight()
    {
        return timeSinceLastSight;
    }

    public void AttackPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = transform.position.y; // Set the Y-coordinate to match the enemy's Y-coordinate
        transform.LookAt(playerPosition);

        if (!alreadyAttacked)
        {
            IEntity entity = player.GetComponent<IEntity>();
            //entity.TakeDamage(enemy.damageAmount);
            entity.TakeDamage(0);
            Debug.Log("Attacked!");
            Debug.Log(playerHealthManager.playerHealth);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemy.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public void TakeHit(DamageType damageType, float damage)
    {
        if (currentState == deadState)
        {
            return;
        }

        if (damageType == DamageType.Small)
        {
            animator.SetTrigger("SmallHit");
            hitState.SetDuration(1.3f);
        }
        else if (damageType == DamageType.Medium)
        {
            animator.SetTrigger("MediumHit");

            hitState.SetDuration(1.4f);
        }
        else if (damageType == DamageType.Big)
        {
            animator.SetTrigger("BigHit");

            hitState.SetDuration(1.5f);
        }

        if (currentState == hitState)
        {
            TakeDamage(damage);
            hitState.EnterState(this);
        }
        else
        {
            TakeDamage(damage);
            TransitionToHit();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log(health);

        if (health <= 0f)
            TransitionToDead();
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        rb.isKinematic = true;
    }

    public void OnEntityDeath()
    {
        Destroy(gameObject);
    }
}
