using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/HumanPuppet")]
public class HumanPuppetSO : ScriptableObject
{
    [Header("HumanPuppet Info")]
    public string name;
    public float hp;
    public float speed;

    [Header("Attack Info")]
    public float damageAmount;
    public float attackRange;
    public float fireRate;
    public float timeBetweenAttacks;
}