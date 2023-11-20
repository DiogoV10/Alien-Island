using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(BaseEnemy enemy);
    void UpdateState(BaseEnemy enemy);
    void ExitState(BaseEnemy enemy);
}
