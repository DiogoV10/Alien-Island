using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponSystem : ScriptableObject
{

    [Header("Info")]
    public string weaponName;
    public Animation weaponAnimations;

    [Header("Attack Info")]
    public float damage;
    public float bulletRange;

    [Header("Ammo Info")]
    public int magSize;
    public int currentAmmo;
    public float attackSpeed;
    public float reloadTime;

    [HideInInspector]
    public bool isReloading;
}
