using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WorldUI : WorldHP
{
    public BaseEnemy _enemy;

    protected override void Awake()
    {
        base.Awake();
        _enemy = GetComponentInParent<BaseEnemy>();
        
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _maxHp = _enemy.CurrentHealth;
    }

    protected override void Update()
    {
        _currentHp = _enemy.CurrentHealth;
        base.Update();
    }
}
