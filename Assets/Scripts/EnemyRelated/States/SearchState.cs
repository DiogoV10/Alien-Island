using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : MonoBehaviour, IEnemyState
{
    [SerializeField] private float searchDuration = 5.0f;
    [SerializeField] private float searchRadius = 10.0f;
    [SerializeField] private float idleDuration = 3.0f;

    private Transform currentWaypoint;
    private float searchTimer;
    private float idleTimer;
    private bool isIdling;

    private Vector3 lastKnownPlayerPosition;
    private Vector3 randomSearchPosition;


    public void EnterState(Enemy enemy)
    {
        searchTimer = 0f;
        idleTimer = 0f;
        isIdling = false;

        lastKnownPlayerPosition = enemy.GetPlayerPosition();

        randomSearchPosition = GetRandomSearchPosition(enemy, lastKnownPlayerPosition);

        SetWaypoint(enemy, randomSearchPosition);
    }

    public void UpdateState(Enemy enemy)
    {
        bool canSeePlayer = enemy.SearchForPlayer();
        if (canSeePlayer)
        {
            enemy.TransitionToChase();
        }

        if (isIdling)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                isIdling = false;

                randomSearchPosition = GetRandomSearchPosition(enemy, lastKnownPlayerPosition);

                SetWaypoint(enemy, randomSearchPosition);
            }
        }
        else
        {
            if (currentWaypoint != null)
            {
                enemy.MoveTo(currentWaypoint.position);

                if (Vector3.Distance(enemy.transform.position, currentWaypoint.position) < 1f)
                {
                    isIdling = true;
                }
            }
        }

        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            enemy.TransitionToWander();
        }
    }

    public void ExitState(Enemy enemy)
    {

    }

    private Vector3 GetRandomSearchPosition(Enemy enemy, Vector3 center)
    {
        Vector2 randomPoint = Random.insideUnitCircle * searchRadius;
        Vector3 targetPosition = new Vector3(randomPoint.x, enemy.transform.position.y, randomPoint.y) + center;
        return targetPosition;
    }

    private void SetWaypoint(Enemy enemy, Vector3 position)
    {
        if (currentWaypoint != null)
        {
            Destroy(currentWaypoint.gameObject);
        }

        currentWaypoint = new GameObject().transform;
        currentWaypoint.position = position;
        enemy.MoveTo(currentWaypoint.position);
    }
}
