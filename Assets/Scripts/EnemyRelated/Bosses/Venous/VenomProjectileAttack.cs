using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomProjectileAttack : MonoBehaviour
{
    [SerializeField] float venomEffectTime = 5f, venomDamageTime = 1.5f;
    public bool attackOn = false;

    [Header("Player Reference")]
    [SerializeField] GameObject target;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] private int playerMask;

    [Header("Attributes")]
    [SerializeField] VenousSO venousSO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (attackOn)
        {
            venomEffectTime -= Time.deltaTime;
            venomDamageTime -= Time.deltaTime;
            if (venomEffectTime > .1f)
            {
                if (venomDamageTime < .1f)
                {
                    DamagePlayer();
                    venomDamageTime = 1.5f;
                }
            }

            else
            {
                venomEffectTime = 5f;
                venomDamageTime = 1.5f;
                attackOn = false;
            }
        }
    }

    void DamagePlayer()
    {
        IEntity entity = target.GetComponent<IEntity>();
        entity.TakeDamage(venousSO.VenomProjectileAttack);
        Debug.Log(playerHealthManager.GetHealth());
    }
}
