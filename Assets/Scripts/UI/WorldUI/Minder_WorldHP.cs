using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minder_WorldHP : WorldHP
{
    public MinderSO _so;

    protected override void Awake()
    {
        base.Awake();
        _maxHp = _so.hp;
    }

    protected override void Update()
    {
        _currentHp = _so.hp;
        base.Update();
    }
}
