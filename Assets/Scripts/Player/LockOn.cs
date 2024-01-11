using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{


    public static LockOn Instance { get; private set; }


    [SerializeField] private float detectionRadius;
    [SerializeField] private Transform targetGameObject;
    [SerializeField] private LayerMask enemyLayer;

    private float offsetPositionY;


    private GameObject lockedEnemy;  // Track the currently locked enemy.


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        offsetPositionY = targetGameObject.position.y;
    }

    private void Update()
    {
        if (lockedEnemy != null)
        {
            BaseEnemy enemy = lockedEnemy.GetComponent<BaseEnemy>();

            if (enemy.IsDead())
            {
                lockedEnemy = null;
            }
        }

        if (lockedEnemy == null || Vector3.Distance(transform.position, lockedEnemy.transform.position) > detectionRadius)
        {
            // Release the lock-on if no enemy is locked or if the locked enemy is out of range.
            lockedEnemy = null;
        }

        Vector3 offsetPosition;

        if (lockedEnemy == null)
        {
            offsetPosition = transform.position + transform.forward * 2.0f;
            offsetPosition.y = PlayerMovement.Instance.transform.position.y + 1.3f;
        }
        else
        {
            // Lock onto the center of the enemy.
            offsetPosition = lockedEnemy.transform.position;
            offsetPosition.y = lockedEnemy.transform.position.y + 1;
        }

        // Set the "target" position to the offset position.
        targetGameObject.position = offsetPosition;

        GameObject closestEnemy = DetectClosestEnemy();
        if (closestEnemy != null && (lockedEnemy == null || closestEnemy != lockedEnemy))
        {
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

            BaseEnemy enemy = collider.GetComponent<BaseEnemy>();

            if (distance < closestDistance && !enemy.IsDead())
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
