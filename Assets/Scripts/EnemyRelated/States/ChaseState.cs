using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonoBehaviour, IEnemyState
{
    [SerializeField] private float chaseDuration = 10.0f;

    private Animator animator;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void EnterState(BaseEnemy enemy)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("SearchIdle", false);
        animator.SetBool("Walk", true);
        animator.Play("Walk");
    }

    public void UpdateState(BaseEnemy enemy)
    {
        enemy.SearchForTarget();

        if (enemy.IsTargetInAttackRange())
        {
            enemy.TransitionToAttack();
        }
        else
        {
            if (enemy.TimeSinceLastSight() >= chaseDuration)
            {
                enemy.TransitionToSearch();
            }

            Vector3 playerPosition = enemy.GetTargetPosition();
            enemy.MoveTo(new Vector3(playerPosition.x, enemy.transform.position.y, playerPosition.z));
        }
    }

    public void ExitState(BaseEnemy enemy)
    {
        
    }
}
