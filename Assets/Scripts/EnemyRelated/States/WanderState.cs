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

    private Transform currentWaypoint;
    private float idleTimer;
    private bool isIdling;

    public void EnterState(Enemy enemy)
    {
        Vector3 yPosition = Vector3.zero;

        idleTimer = 0f;
        isIdling = false;

        if (useWaypoints && waypoints != null && waypoints.Length > 0)
        {
            currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];

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

    public void UpdateState(Enemy enemy)
    {
        bool canSeePlayer = enemy.SearchForPlayer();
        if (canSeePlayer)
        {
            enemy.TransitionToAttack();
        }

        if (isIdling)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                isIdling = false;
                EnterState(enemy);
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
                }
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        if (!useWaypoints && currentWaypoint != null)
        {
            GameObject.Destroy(currentWaypoint.gameObject);
        }
    }
}
