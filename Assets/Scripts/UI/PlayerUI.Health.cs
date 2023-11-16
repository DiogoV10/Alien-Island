using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public partial class PlayerUI : MonoBehaviour //Health
{
    [Header("Health References")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private float _sliderLerpTime = 0.2f;
    private PlayerHealthManager _health;
    private float _playerMaxHealth;
    

    private void ShowHealth(float health)
    {
        if (!_healthSlider) return;

        _healthSlider.DOValue(health / _playerMaxHealth, _sliderLerpTime);


    }
}
