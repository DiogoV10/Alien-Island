using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/MeleeWeapon")]
public class MeleeWeaponSO : ScriptableObject
{
    public new string name;
    public GameObject weaponPrefab;
    public float damage;
    public float attackSpeed;
    public Sprite icon;
}
