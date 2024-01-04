using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{

    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] int playerMask;
    [SerializeField] int groundMask;
    //private bool isPlayerInAttackRange;

    [Header("Attributes")]
    [SerializeField] MinderSO minderSO;
    [SerializeField] Minder minder;

    //ObjectBombingAttack damageArea;
    Collider collider;
    float distToPlayer;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (minder.ObjectThrowAttackOn && distToPlayer < 2f) collider.isTrigger = true;
        else collider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (minder.ObjectThrowAttackOn)
        {
            //collider.isTrigger = true;
            if (other.gameObject.layer == playerMask)
            {
                IEntity entity = player.GetComponent<IEntity>();
                entity.TakeDamage(minderSO.LevitateAttackDamage);
            }
        }
    }
}
