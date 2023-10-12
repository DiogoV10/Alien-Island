using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{


    public static LockOn Instance { get; private set; }


    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask enemyLayer;


    private GameObject lockedEnemy;  // Track the currently locked enemy.


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (lockedEnemy == null || Vector3.Distance(transform.position, lockedEnemy.transform.position) > detectionRadius)
        {
            // Release the lock-on if no enemy is locked or if the locked enemy is out of range.
            lockedEnemy = null;
        }
        else
        {
            // Continue to lock onto the current enemy.
            // You can add your logic for targeting here.
        }

        GameObject closestEnemy = DetectClosestEnemy();
        if (closestEnemy != null && (lockedEnemy == null || closestEnemy != lockedEnemy))
        {
            // Lock onto the closest enemy if no enemy is currently locked or if a new enemy is closer.
            lockedEnemy = closestEnemy;
        }
    }

    private GameObject DetectClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.gameObject;
            }
        }

        return closestEnemy;
    }

    public GameObject GetLockedEnemy()
    {
        return lockedEnemy;
    }
}
