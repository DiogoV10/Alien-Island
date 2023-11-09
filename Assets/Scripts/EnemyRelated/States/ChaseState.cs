using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonoBehaviour, IEnemyState
{
    [SerializeField] private float chaseDuration = 10.0f;

    public void EnterState(Enemy enemy)
    {
        
    }

    public void UpdateState(Enemy enemy)
    {
        enemy.SearchForPlayer();

        if (enemy.IsPlayerInAttackRange())
        {
            enemy.TransitionToAttack();
        }
        else
        {
            if (enemy.TimeSinceLastSight() >= chaseDuration)
            {
                enemy.TransitionToSearch();
            }

            Vector3 playerPosition = enemy.GetPlayerPosition();
            enemy.MoveTo(new Vector3(playerPosition.x, enemy.transform.position.y, playerPosition.z));
        }
    }

    public void ExitState(Enemy enemy)
    {
        
    }
}
