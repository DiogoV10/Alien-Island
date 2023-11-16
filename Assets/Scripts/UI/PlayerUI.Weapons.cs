using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerUI : MonoBehaviour // Weapons
{
    [Header("WeaponUI")]
    [SerializeField] private Transform _weapon1;
    [SerializeField] private Transform _weapon2;

    private Image _weapon1_Image;
    private Image _weapon2_Image;

    [Header("Não é bom")]
    public GameObject RadioText;

    private void SetMeleeWeapon(MeleeWeaponSO meleeWeaponSO)
    {
        if (meleeWeaponSO != null)
        {
            //_weapon1_Image.sprite = meleeWeaponSO.icon; //ToDo: Ana
        }
    }

    private void SetRangedWeapon(RangedWeaponSO rangedWeaponSO)
    {
        if (rangedWeaponSO != null)
        {
            //_weapon2_Image.sprite = rangedWeaponSO.icon; //ToDo: Ana
        }
    }
}
