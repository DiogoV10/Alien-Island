using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Pupetteer")]
public class PupeteerSO : ScriptableObject
{
    [Header("Pupetteer Info")]
    public string name;
    public float hp;
    public float speed;

    [Header("Attack Info")]
    public float damageAmount;
    public float attackRange;
    //public float sightRange;
    //public float fireRate;
    public float timeBetweenAttacks;
}