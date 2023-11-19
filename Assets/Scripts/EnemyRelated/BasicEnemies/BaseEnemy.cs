using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour, IEntity
{
    public enum DamageType
    {
        Small,
        Medium,
        Big,
    }

    [Header("EnemySO reference")]
    [SerializeField] protected EnemySO enemy;

    [Header("Enemy States")]
    [SerializeField] protected WanderState wanderState;
    [SerializeField] protected SearchState searchState;
    [SerializeField] protected ChaseState chaseState;
    [SerializeField] protected AttackState attackState;
    [SerializeField] protected HitState hitState;
    [SerializeField] protected DeadState deadState;

    [Header("Other Refs")]
    [SerializeField] protected LayerMask whatIsTarget;

    protected Rigidbody rb;
    protected CapsuleCollider capsuleCollider;
    protected Animator animator;
    protected GameObject player;
    protected GameObject target;
    protected NavMeshAgent navMeshAgent;

    protected Vector3 oldDestination;

    protected float lastSightTime;
    protected float timeSinceLastSight;

    protected float health;

    protected bool isTargetInSightRange, isTargetInAttackRange, alreadyAttacked;
    protected bool canSeeTarget;

    protected IEnemyState currentState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        rb.isKinematic = false;
        capsuleCollider.isTrigger = false;

        wanderState.Initialize(animator);
        deadState.Initialize(animator);
        searchState.Initialize(animator);
        chaseState.Initialize(animator);
    }

    protected virtual void Start()
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

        player = GameObject.FindGameObjectWithTag("Player");
        target = player;

        navMeshAgent.speed = enemy.speed;
    }

    protected virtual void Update()
    {
        currentState.UpdateState(this);
    }

    protected void ChangeState(IEnemyState newState)
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

    public bool SearchForTarget()
    {
        if (target != null)
        {

            isTargetInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, whatIsTarget);

            if (isTargetInSightRange)
            {
                if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out RaycastHit hit, enemy.sightRange))
                {
                    if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Target"))
                    {
                        lastSightTime = Time.time;
                        canSeeTarget = true;
                    }
                    else
                    {
                        canSeeTarget = false;
                    }
                }
            }
            else
            {
                canSeeTarget = false;
            }
        }

        timeSinceLastSight = Time.time - lastSightTime;

        return canSeeTarget;
    }

    public Vector3 GetTargetPosition()
    {
        if (target != null)
        {
            return target.transform.position;
        }
        return Vector3.zero;
    }

    public bool IsTargetInAttackRange()
    {
        return isTargetInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, whatIsTarget);
    }

    public float TimeSinceLastSight()
    {
        return timeSinceLastSight;
    }

    public void StartNavigation()
    {
        navMeshAgent.isStopped = false;
    }

    public void StopNavigation()
    {
        navMeshAgent.isStopped = true;
    }

    public abstract void MoveTo(Vector3 destination);

    public abstract void AttackTarget();

    public abstract void TakeHit(Enemy.DamageType damageType, float damage);

    public abstract void TakeDamage(float damage);

    public abstract void Die();

    public abstract void OnEntityDeath();
}

