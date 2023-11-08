using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{

    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] private int playerMask;
    [SerializeField] private int groundMask;
    //private bool isPlayerInAttackRange;

    [Header("Attributes")]
    [SerializeField] MinderSO minderSO;
    [SerializeField] Minder minder;

    //ObjectBombingAttack damageArea;
    Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (minder.ObjectThrowAttackOn) collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (minder.ObjectThrowAttackOn)
        {
            collider.isTrigger = true;
            if (other.gameObject.layer == playerMask)
            {
                IEntity entity = player.GetComponent<IEntity>();
                entity.TakeDamage(minderSO.LevitateAttackDamage);
            }
        }
    }
}
