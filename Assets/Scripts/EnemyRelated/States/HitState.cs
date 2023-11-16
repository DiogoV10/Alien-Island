using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : MonoBehaviour, IEnemyState
{
    private int hitCount = 0;
    private float hitTimer = 0f;
    private float hitDuration = 3.0f;

    public void EnterState(BaseEnemy enemy)
    {
        hitCount++;
        hitTimer = 0f;
    }

    public void UpdateState(BaseEnemy enemy)
    {
        hitTimer += Time.deltaTime;
        if (hitTimer >= hitDuration)
        {
            hitCount = 0;
            hitTimer = 0;

            if (enemy.IsTargetInAttackRange())
                enemy.TransitionToAttack();
            else
                enemy.TransitionToChase();
        }
    }

    public void ExitState(BaseEnemy enemy)
    {
        
    }

    public void SetDuration(float duration)
    {
        hitDuration = duration;
    }
}
