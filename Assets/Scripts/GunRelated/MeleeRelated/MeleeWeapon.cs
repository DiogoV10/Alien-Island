using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{


    public static MeleeWeapon Instance { get; private set; }


    [SerializeField] private float hitRadius = 1f;

    private List<Collider> enemies = new List<Collider>();
    private bool canHit = false;
    bool playSound = true;

    public static event EventHandler OnHitEnemy;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!canHit) return;

        enemies.Clear();
        Vector3 hitPosition = PlayerCombat.Instance.transform.position + PlayerCombat.Instance.transform.forward * hitRadius;

        Collider[] colliders = Physics.OverlapSphere(hitPosition, hitRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemies.Add(collider);
                HitEnemy(collider);
                OnHitEnemy?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!canHit) return;

    //    bool hasHit = false;

    //    if (other.CompareTag("Enemy"))
    //    {
    //        if (enemies != null)
    //        {
    //            for (int i = 0; i < enemies.Count; i++)
    //            {
    //                if (other == enemies[i])
    //                {
    //                    hasHit = true;
    //                }
    //            }
    //        }
            
    //        if (!hasHit) 
    //        { 
    //            HitEnemy(other);
    //        }
    //    }
    //}

    private void HitEnemy(Collider enemyCollider)
    {
        Debug.Log("Hit");
        enemyCollider.GetComponent<HumanSummonAttack>()?.TakeDamage(MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<Enemy>()?.TakeHit(PlayerCombat.Instance.GetDamageType(), MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<Tank>()?.TakeHit(PlayerCombat.Instance.GetDamageType(), MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<Bullseye>()?.TakeHit(PlayerCombat.Instance.GetDamageType(), MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<PupeteerMeleeAttack>()?.TakeDamage(MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<Minder>()?.TakeDamage(MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());
        enemyCollider.GetComponent<Venous>()?.TakeDamage(MeleeWeaponsSelector.Instance.GetActiveWeaponDamage());

        canHit = false;
    }

    public void SetCanHit(bool hit)
    {
        canHit = hit;
    }
}
