using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Bullseye : BaseEnemy, IEntity
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private EnemyAudio enemyAudio;

    override protected void Awake()
    {
        base.Awake();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    public override void MoveTo(Vector3 destination)
    {
        if (destination != oldDestination)
        {
            StartNavigation();
            navMeshAgent.SetDestination(destination);
            oldDestination = destination;
        }
    }

    public override void AttackTarget()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.y = targetPosition.y + target.transform.position.y * 0.5f;
        transform.LookAt(targetPosition);

        if (!alreadyAttacked)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.SetDamageAndMove(enemy.damageAmount - PlayerCombat.Instance.GetHealthReduction(), targetPosition - projectileSpawnPoint.position);

            animator.SetTrigger("Punch");
            animator.CrossFade("Shoot",0.1f);

            enemyAudio?.PlayAttackSound();

            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), enemy.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public override void TakeHit(DamageType damageType, float damage)
    {
        if (currentState == deadState)
        {
            return;
        }

        if (damageType == DamageType.Small)
        {
            animator.SetTrigger("SmallHit");
            hitState.SetDuration(1.3f);
        }
        else if (damageType == DamageType.Medium)
        {
            animator.SetTrigger("MediumHit");
            hitState.SetDuration(1.4f);
        }
        else if (damageType == DamageType.Big)
        {
            animator.SetTrigger("BigHit");
            hitState.SetDuration(1.5f);
        }

        if (currentState == hitState)
        {
            TakeDamage(damage);

            if (currentState != deadState)
            {
                hitState.EnterState(this);
            }
        }
        else
        {
            TakeDamage(damage);

            if (currentState != deadState)
            {
                TransitionToHit();
            }
        }
    }

    public bool CanSeeTarget()
    {
        return canSeeTarget;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetTargetToPlayer()
    {
        target = player;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (enemyAudio != null) enemyAudio.PlayHurtSound();

        Debug.Log(health);

        if (health <= 0f)
        {
            SkillPoints.Instance.IncreaseEnemyKilledPoints(200);
            TransitionToDead();
        }    
    }

    public override void Die()
    {
        Debug.Log("Enemy Died!");
        rb.isKinematic = true;
        capsuleCollider.isTrigger = true;
    }

    public override void OnEntityDeath()
    {
        Destroy(gameObject);
    }
}
