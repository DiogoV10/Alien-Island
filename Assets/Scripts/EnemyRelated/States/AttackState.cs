using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IEnemyState
{
    public void EnterState(Enemy enemy)
    {

    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.IsPlayerInAttackRange())
            enemy.AttackPlayer();
        else
            enemy.TransitionToChase();
    }

    public void ExitState(Enemy enemy)
    {

    }
}
