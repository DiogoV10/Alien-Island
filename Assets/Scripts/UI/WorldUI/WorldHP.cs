using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldHP : MonoBehaviour
{
    protected float _maxHp = 100f;
    protected float _currentHp = 100f;
    protected float _hpLastFrame;


    protected Slider _hpSlider;

    protected virtual void Awake()
    {
        _hpSlider = GetComponentInChildren<Slider>();
    }

    protected virtual void Update()
    {

        SetHP();


        _hpLastFrame = _currentHp;
        if (_currentHp <= 0)
        {
            _hpSlider.gameObject.SetActive(false);
            
            DOVirtual.DelayedCall(1.5f, () => Destroy(gameObject));
        }
    }

    protected virtual void SetHP()
    {
        _hpSlider.value = _currentHp / _maxHp;
    }

}
