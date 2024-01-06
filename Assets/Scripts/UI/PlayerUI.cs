using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerUI : MonoBehaviour //Base
{
    [Header("GeneralReference")]
    [SerializeField] private Transform _player;

    private void Awake()
    {
        _health = _player.GetComponent<PlayerHealthManager>();
        _playerMaxHealth = _health.GetMaxHealth();
        _playerSkills = _player.GetComponent<PlayerSkills>();
        SetSliders();
        SetWeaponSwapElements();
    }

    private void OnEnable()
    {
        if (_health)
        {
            _health.OnHealthChanged += ShowHealth;
            ShowHealth(_health.GetHealth());
        }
        if (_playerSkills)
        {
            _playerSkills.OnCastSkill += StartSkillCooldown;
            _playerSkills.OnCastUltimate += StartUltimateCooldown;
        }

        WeaponSelector.Instance.OnMeleeEquiped += WeaponSelector_OnMeleeEquiped;
        WeaponSelector.Instance.OnRangedEquiped += WeaponSelector_OnRangedEquiped;
        

    }

    private void WeaponSelector_OnRangedEquiped(RangedWeaponSO rangedWeaponSO)
    {
        SetRangedWeapon(rangedWeaponSO);
    }

    private void WeaponSelector_OnMeleeEquiped(MeleeWeaponSO meleeWeaponSO)
    {
        SetMeleeWeapon(meleeWeaponSO);
    }

    private void OnDisable()
    {
        if (_health)
        {
            _health.OnHealthChanged -= ShowHealth;
        }
        if (_playerSkills)
        {
            _playerSkills.OnCastSkill -= StartSkillCooldown;
            _playerSkills.OnCastUltimate -= StartUltimateCooldown;
        }

        WeaponSelector.Instance.OnMeleeEquiped -= SetMeleeWeapon;
        WeaponSelector.Instance.OnRangedEquiped -= SetRangedWeapon;
    }

}