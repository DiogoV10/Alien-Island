using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Venous")]
public class VenousSO : ScriptableObject
{
    [Header("Venous Info")]
    public string name;
    public float hp;

    [Header("Attack Info")]
    public float fireRate;
    public float venomSmokeAttack;
    public float VenomProjectileAttack;
}
