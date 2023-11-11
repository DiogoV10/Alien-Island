using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : MonoBehaviour, IEnemyState
{
    private int hitCount = 0;
    private float hitTimer = 0f;
    private float hitDuration = 3.0f;

    public void EnterState(Enemy enemy)
    {
        hitCount++;
        hitTimer = 0f;
    }

    public void UpdateState(Enemy enemy)
    {
        hitTimer += Time.deltaTime;
        if (hitTimer >= hitDuration)
        {
            hitCount = 0;
            hitTimer = 0;

            if (enemy.IsPlayerInAttackRange())
                enemy.TransitionToAttack();
            else
                enemy.TransitionToChase();
        }
    }

    public void ExitState(Enemy enemy)
    {
        
    }

    public void SetDuration(float duration)
    {
        hitDuration = duration;
    }
}
