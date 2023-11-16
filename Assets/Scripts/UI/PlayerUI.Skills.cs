using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public partial class PlayerUI : MonoBehaviour //Skills
{
    [Header("Skill references")]
    [SerializeField] private Transform _skill_T;
    [SerializeField] private Transform _ultimate_T;
    [SerializeField] private Transform _ultimate2_T;

    private Slider _skill_S;
    private Slider _ultimate_S;
    private Slider _ultimate2_S;
    private PlayerSkills _playerSkills;

    private void StartSkillCooldown()
    {
        _skill_S.DOValue(1, _playerSkills.SkillCooldownTime);
        DOVirtual.DelayedCall(_playerSkills.SkillCooldownTime, () => _skill_S.value = 0);
    }
    private void StartUltimateCooldown()
    {
        _ultimate_S.DOValue(1, _playerSkills.UltimateCooldownTime);
        DOVirtual.DelayedCall(_playerSkills.UltimateCooldownTime, () => _ultimate_S.value = 0);

        
        _ultimate2_S.DOValue(1, _playerSkills.UltimateCooldownTime);
        DOVirtual.DelayedCall(_playerSkills.UltimateCooldownTime, () => _ultimate2_S.value = 0);

    }

    private void SetSliders()
    {
        _skill_S = _skill_T.GetComponentInChildren<Slider>();
        _ultimate_S = _ultimate_T.GetComponentInChildren<Slider>();
        _ultimate2_S = _ultimate2_T.GetComponentInChildren<Slider>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartUltimateCooldown();
            StartSkillCooldown();
        }
    }
}
