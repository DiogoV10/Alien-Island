using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun/New Gun or Weapon" )]
public class GunData : ScriptableObject
{
    [Header("Gun Info")]
    public string gunOrWeaponName;
    public bool isMelee;
    public float range;


}
