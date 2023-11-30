using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Tank : BaseEnemy, IEntity
{

    private int hitCount = 0;
    private int smallHitCount = 5;
    private int mediumHitCount = 10;
    private int bigHitCount = 15;
    private float hitTimer = 0f;
    private float maxHitTimer = 5f;


    protected override void Update()
    {
        base.Update();

        hitTimer += Time.deltaTime;

        if (hitTimer >= maxHitTimer)
        {
            hitCount = 0;
            hitTimer = 0f;
        }
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
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Punch");
            IEntity entity = target.GetComponent<IEntity>();
            entity.TakeDamage(enemy.damageAmount);
            Debug.Log("Attacked!");
            Debug.Log(player.GetComponent<PlayerHealthManager>().GetHealth());
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
            hitCount++;
        }
        else if (damageType == DamageType.Medium)
        {
            hitCount += 2;
        }
        else if (damageType == DamageType.Big)
        {
            hitCount += 3;
        }

        if (hitCount >= bigHitCount)
        {
            animator.SetTrigger("BigHit");
            hitState.SetDuration(1.5f);

            hitCount = 0;
            hitTimer = 0f;
        }
        else if (hitCount >= mediumHitCount)
        {
            animator.SetTrigger("MediumHit");
            hitState.SetDuration(1.4f);
        }
        else if (hitCount >= smallHitCount)
        {
            animator.SetTrigger("SmallHit");
            hitState.SetDuration(1.3f);
        }

        if (currentState == hitState)
        {
            TakeDamage(damage);
            hitState.EnterState(this);
        }
        else
        {
            TakeDamage(damage);
            TransitionToHit();
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

        Debug.Log(health);

        if (health <= 0f)
            TransitionToDead();
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
