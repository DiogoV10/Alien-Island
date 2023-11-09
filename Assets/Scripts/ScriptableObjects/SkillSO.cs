using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/NormalSkill")]
public class SkillSO : ScriptableObject
{
    public new string name;
    public int damage;
    public float duration;
    public float range;
    public LayerMask affectedLayers;
    public Sprite icon;
}


