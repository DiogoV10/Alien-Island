using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Combo")]
public class ComboSO : ScriptableObject
{
    public List<AttackSO> combo;
    public ComboType comboType;
    public string weaponName;
    public float nextComboDelay;
}

public enum ComboType
{
    Air,
    Hold,
    Wait,
    Normal,
}
