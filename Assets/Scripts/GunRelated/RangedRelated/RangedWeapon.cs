using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedWeapon : MonoBehaviour
{


    public static RangedWeapon Instance { get; private set; }

    [SerializeField] private Transform muzzle;

    [Header("Shooting Info")]
    [SerializeField] private float gunDamage;

    [SerializeField] private GameObject hitPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Shoot()
    {
        if (PlayerMovement.Instance.ShouldFaceObject())
        {
            GameObject lockedEnemy = LockOn.Instance.GetLockedEnemy();
            if (lockedEnemy != null /*&& !IsEnemyBehindObstacle(lockedEnemy)*/)
            {
                lockedEnemy.GetComponent<Enemy>()?.TakeHit(Enemy.DamageType.Small, gunDamage);
                Debug.Log("Hit");
            }
            else
            {
                int layerMask = ~LayerMask.GetMask("Player");
                RaycastHit hit;
                if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log("Hit something with tag: " + hit.transform.tag);
                    InstantiateHitEffect(hit.point);
                }
            }
        }
        else
        {
            int layerMask = ~LayerMask.GetMask("Player");
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Hit something with tag: " + hit.transform.tag);
                InstantiateHitEffect(hit.point);
            }
        }
    }

    private bool IsEnemyBehindObstacle(GameObject enemy)
    {
        if (enemy != null)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();

            if (enemyComponent != null)
            {
                return !enemyComponent.CanSeePlayer();
            }
        }

        return false;
    }

    private void InstantiateHitEffect(Vector3 position)
    {
        Instantiate(hitPrefab, position, Quaternion.identity);
    }

    public float GunDamage()
    {
        return gunDamage;
    }
}
