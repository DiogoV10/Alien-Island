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

        deadTimer = 0f;
    }

    public void EnterState(BaseEnemy enemy)
    {
        animator.SetTrigger("Dead");

        enemy.SetIsDead();

        PlayerUpgrades.Instance.AddUpgradePoint();

        deadTimer = 0f;

        enemy.StopNavigation();
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
