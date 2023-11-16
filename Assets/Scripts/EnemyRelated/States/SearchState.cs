using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : MonoBehaviour, IEnemyState
{
    [SerializeField] private float searchDuration = 5.0f;
    [SerializeField] private float searchRadius = 10.0f;
    [SerializeField] private float idleDuration = 3.0f;

    private Animator animator;
    private Transform currentWaypoint;
    private float searchTimer;
    private float idleTimer;
    private float nextIdleActTime;
    private bool isIdling;

    private Vector3 lastKnownPlayerPosition;
    private Vector3 randomSearchPosition;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void EnterState(BaseEnemy enemy)
    {
        searchTimer = 0f;
        idleTimer = 0f;
        isIdling = false;

        lastKnownPlayerPosition = enemy.GetTargetPosition();

        randomSearchPosition = GetRandomSearchPosition(enemy, lastKnownPlayerPosition);

        SetWaypoint(enemy, randomSearchPosition);

        animator.SetBool("SearchIdle", false);
        animator.SetBool("Walk", true);

        nextIdleActTime = Time.time + Random.Range(5f, 10f);
    }

    public void UpdateState(BaseEnemy enemy)
    {
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            enemy.TransitionToWander();
        }

        bool canSeePlayer = enemy.SearchForTarget();
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

                animator.SetBool("SearchIdle", false);
                animator.SetBool("Walk", true);
                animator.Play("Walk");
            }
            else if (Time.time >= nextIdleActTime)
            {
                int randomAnimation = Random.Range(0, 2);

                if (randomAnimation < 1)
                {
                    animator.SetTrigger("SearchIdleAct");

                    Debug.Log("Search Idle");
                }

                nextIdleActTime = Time.time + Random.Range(5f, 10f);
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
                    animator.SetBool("Walk", false);
                    animator.SetBool("SearchIdle", true);
                    animator.Play("Search Idle");
                }
            }
        }
    }

    public void ExitState(BaseEnemy enemy)
    {
        animator.SetBool("SearchIdle", false);
    }

    private Vector3 GetRandomSearchPosition(BaseEnemy enemy, Vector3 center)
    {
        Vector2 randomPoint = Random.insideUnitCircle * searchRadius;
        Vector3 targetPosition = new Vector3(randomPoint.x, enemy.transform.position.y, randomPoint.y) + center;
        return targetPosition;
    }

    private void SetWaypoint(BaseEnemy enemy, Vector3 position)
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
