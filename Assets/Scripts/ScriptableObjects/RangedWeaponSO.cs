using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/RangedWeapon")]
public class RangedWeaponSO : ScriptableObject
{
    public new string name;
    public GameObject weaponPrefab;
    public int damage;
    public float attackSpeed;
    public float range;
    public Sprite icon;
    public AudioClip[] attackSounds;
    [Range(0f,1f)]
    public float[] attackSoundsVolume;
}
