using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponSystem : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public Animation animations;

    [Header("Attack Info")]
    public float damage;
    public float attackRange;

    [Header("Weapon Misc")]
    public float attackSpeed;
    public float critChance;

    [HideInInspector]
    public bool canAttack;
}
