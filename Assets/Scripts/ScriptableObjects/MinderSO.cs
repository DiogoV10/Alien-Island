using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Minder")]
public class MinderSO : ScriptableObject
{
    [Header("Minder Info")]
    public string name;
    public float hp;

    [Header("Attack Info")]
    public float fireRate;
    public float LevitateAttackDamage;
    public float BombingAttackDamage;
}
