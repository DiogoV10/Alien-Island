using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : MonoBehaviour, IEnemyState
{
    private Animator animator;

    private float deadTimer = 0f;
    [SerializeField] private float deadDuration = 3.0f;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void EnterState(BaseEnemy enemy)
    {
        animator.SetTrigger("Dead");

        deadTimer = 0f;

        enemy.Die();
    }

    public void UpdateState(BaseEnemy enemy)
    {
        deadTimer += Time.deltaTime;

        if (deadTimer >= deadDuration)
        {
            ExitState(enemy);
        }
    }

    public void ExitState(BaseEnemy enemy)
    {
        enemy.OnEntityDeath();
    }
}
