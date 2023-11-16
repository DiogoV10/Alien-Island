using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : BaseEnemy, IEntity
{
    

    public override void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, enemy.speed * Time.deltaTime);
        transform.LookAt(destination);
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
            Debug.Log(player.GetComponent<PlayerHealthManager>().playerHealth);
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
