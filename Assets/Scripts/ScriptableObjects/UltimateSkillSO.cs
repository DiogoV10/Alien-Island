using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/UltimateSkill")]
public class UltimateSkillSO : ScriptableObject
{
    public new string name;
    public Weapons weapon;
    public int damage;
    public Enemy.DamageType damageType;
    public int cooldown;
    public Sprite icon;
    public bool unlocked;
}

public enum Weapons
{
    Knife,
    Katana,
    Pistol,
    Rifle,
}