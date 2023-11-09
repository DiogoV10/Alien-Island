using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/UltimateSkill")]
public class UltimateSkillSO : ScriptableObject
{
    public new string name;
    public Weapons weapon;
    public int damage;
    public Sprite icon;
}

public enum Weapons
{
    Knife,
    Katana,
    Pistol,
    Rifle,
}