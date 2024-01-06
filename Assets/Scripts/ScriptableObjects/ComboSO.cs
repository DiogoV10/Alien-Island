using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Combo")]
public class ComboSO : ScriptableObject
{
    public List<AttackSO> combo;
    public PlayerCombat.Weapons weapon;
}
