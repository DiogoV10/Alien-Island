using System;
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


    public static event EventHandler OnHitEnemy;

    private void Awake()
    {
        Instance = this;
    }

    public void Shoot()
    {
        if (muzzle == null)
            return;

        if (PlayerMovement.Instance.ShouldFaceObject())
        {
            GameObject lockedEnemy = LockOn.Instance.GetLockedEnemy();
            if (lockedEnemy != null /*&& !IsEnemyBehindObstacle(lockedEnemy)*/)
            {
                lockedEnemy.GetComponent<HumanSummonAttack>()?.TakeDamage(RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<Enemy>()?.TakeHit(BaseEnemy.DamageType.Small, RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<Tank>()?.TakeHit(BaseEnemy.DamageType.Small, RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<Bullseye>()?.TakeHit(BaseEnemy.DamageType.Small, RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<PupeteerMeleeAttack>()?.TakeDamage(RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<Minder>()?.TakeDamage(RangedWeaponsSelector.Instance.GetActiveWeaponDamage());
                lockedEnemy.GetComponent<Venous>()?.TakeDamage(RangedWeaponsSelector.Instance.GetActiveWeaponDamage());

                OnHitEnemy?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                int layerMask = ~LayerMask.GetMask("Player");
                RaycastHit hit;
                if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("Hit something with tag: " + hit.transform.tag);
                        InstantiateHitEffect(hit.point);
                    }
                }
            }
        }
        else
        {
            int layerMask = ~LayerMask.GetMask("Player");
            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Hit something with tag: " + hit.transform.tag);
                    InstantiateHitEffect(hit.point);
                }
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
                return !enemyComponent.CanSeeTarget();
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
