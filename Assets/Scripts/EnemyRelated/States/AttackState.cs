using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour, IEnemyState
{
    private float transitionDelay = 0.5f;
    private float elapsedTime = 0f;

    public void EnterState(BaseEnemy enemy)
    {

    }

    public void UpdateState(BaseEnemy enemy)
    {
        elapsedTime += Time.deltaTime;

        if (enemy.IsTargetInAttackRange())
        {
            elapsedTime = 0f;
            enemy.AttackTarget();
        }
        else
        {
            if (elapsedTime >= transitionDelay)
            {
                enemy.TransitionToChase();
            }
        }
    }

    public void ExitState(BaseEnemy enemy)
    {

    }
}
