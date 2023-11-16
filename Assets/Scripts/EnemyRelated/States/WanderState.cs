using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WanderState : MonoBehaviour, IEnemyState
{
    [SerializeField] private bool useWaypoints = true;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float wanderZoneRadius = 10f;
    [SerializeField] private float idleDuration = 3.0f;

    private Animator animator;
    private Transform currentWaypoint;
    private float idleTimer;
    private float nextIdleActTime;
    private bool isIdling;
    private int currentWaypointIndex;
    private int nextWaypointIndex;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void EnterState(BaseEnemy enemy)
    {
        Vector3 yPosition = Vector3.zero;

        idleTimer = 0f;
        isIdling = false;
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
        animator.Play("Walk");

        if (useWaypoints && waypoints != null && waypoints.Length > 0)
        {
            if (waypoints.Length > 1)
            {
                do
                {
                    nextWaypointIndex = Random.Range(0, waypoints.Length);
                } while ((nextWaypointIndex == currentWaypointIndex));
            }
            else { nextWaypointIndex = 0; }

            currentWaypoint = waypoints[nextWaypointIndex];

            currentWaypointIndex = nextWaypointIndex;

            yPosition = new Vector3(currentWaypoint.position.x, enemy.transform.position.y, currentWaypoint.position.z);

            currentWaypoint.position = yPosition;
        }
        else
        {
            Vector2 randomPoint = Random.insideUnitCircle * wanderZoneRadius;
            Vector3 targetPosition = new Vector3(randomPoint.x, enemy.transform.position.y, randomPoint.y) + enemy.transform.position;

            currentWaypoint = new GameObject().transform;
            currentWaypoint.position = targetPosition;
        }
    }

    public void UpdateState(BaseEnemy enemy)
    {
        bool canSeePlayer = enemy.SearchForTarget();
        if (canSeePlayer)
        {
            if (!enemy.IsTargetInAttackRange())
            {
                enemy.TransitionToChase();
            }
            else
            {
                enemy.TransitionToAttack();
            }
        }

        if (isIdling)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                isIdling = false;
                EnterState(enemy);
            }
            else if (Time.time >= nextIdleActTime) 
            { 
                int randomAnimation = Random.Range(0, 3);

                if (randomAnimation < 2)
                {
                    animator.SetTrigger($"IdleAct{randomAnimation + 1}");

                    Debug.Log("Idle");
                }

                nextIdleActTime = Time.time + Random.Range(10f, 15f);
            }
        }
        else
        {
            if (currentWaypoint != null)
            {
                enemy.MoveTo(currentWaypoint.position);

                if (Vector3.Distance(enemy.transform.position, currentWaypoint.position) < 0.1f)
                {
                    isIdling = true;
                    animator.SetBool("Idle", true);
                    animator.SetBool("Walk", false);
                }
            }
        }
    }

    public void ExitState(BaseEnemy enemy)
    {
        if (!useWaypoints && currentWaypoint != null)
        {
            Destroy(currentWaypoint.gameObject);
        }
    }
}
