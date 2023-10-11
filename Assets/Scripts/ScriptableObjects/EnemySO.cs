using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;
    public float enemyHP;
    public float speed;

    [Header("Attack Info")]
    public float damageAmount;
    public float attackRange;
    public float sightRange;
    public float fireRate;
    public float timeBetweenAttacks;
}
