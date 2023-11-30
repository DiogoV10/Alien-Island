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

        MeleeWeaponsSelector.Instance.OnChangeWeapon += SetMeleeWeapon;
        RangedWeaponsSelector.Instance.OnChangeWeapon += SetRangedWeapon;
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

        MeleeWeaponsSelector.Instance.OnChangeWeapon -= SetMeleeWeapon;
        RangedWeaponsSelector.Instance.OnChangeWeapon -= SetRangedWeapon;
    }
}
