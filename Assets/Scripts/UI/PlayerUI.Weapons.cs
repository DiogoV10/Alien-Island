using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class PlayerUI : MonoBehaviour // Weapons
{
    [Header("WeaponUI")]
    [SerializeField] private Image _weapon1_Image;
    [SerializeField] private Image _weapon2_Image;

    [Header("Weapon Swapping")]
    [SerializeField] private Transform _weapon1_t;
    [SerializeField] private Transform _weapon2_t;
    public float _swapTime = 0.2f;
    private Vector2 _equipedWeapon_pos;
    private Vector2 _offWeapon_pos;
    bool _firstWeaponEquiped = true;


    [Header("Não é bom")]
    public GameObject RadioText;

    private void SetWeaponSwapElements()
    {
        _equipedWeapon_pos = _weapon1_t.GetComponent<RectTransform>().position;
        _offWeapon_pos = _weapon2_t.GetComponent<RectTransform>().position;
    }

    private void SetMeleeWeapon(MeleeWeaponSO meleeWeaponSO)
    {
        if (meleeWeaponSO != null)
        {
            _weapon1_Image.sprite = meleeWeaponSO.icon;
            SwapWeapons();
        }
    }

    private void SetRangedWeapon(RangedWeaponSO rangedWeaponSO)
    {
        if (rangedWeaponSO != null)
        {
            _weapon2_Image.sprite = rangedWeaponSO.icon;
            SwapWeapons();
        }
    }

    private void SwapWeapons()
    {
        if (_firstWeaponEquiped)
        {
            _weapon1_t.DOMove(_offWeapon_pos, _swapTime);
            _weapon1_t.DOScale(0.5f, _swapTime);

            _weapon2_t.DOMove(_equipedWeapon_pos, _swapTime);
            _weapon2_t.DOScale(1, _swapTime);
        }
        else
        {
            _weapon2_t.DOMove(_offWeapon_pos, _swapTime);
            _weapon2_t.DOScale(0.5f, _swapTime);

            _weapon1_t.DOMove(_equipedWeapon_pos, _swapTime);
            _weapon1_t.DOScale(1, _swapTime);
        }
        _firstWeaponEquiped = !_firstWeaponEquiped;
    }
}
