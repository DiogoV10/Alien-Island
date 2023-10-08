using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Normal Attack")]
public class AttackSO : ScriptableObject
{
    public string attackName;
    public AnimationClip animationClip;
    public float damage;
    public float nextAttackDelay;
}
