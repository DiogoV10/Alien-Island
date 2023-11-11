using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBlast : MonoBehaviour
{
    private SkillSO skill;

    private float startTime;
    private float duration;
    private float damageInterval = 1.0f;


    private void Start()
    {
        startTime = Time.time;
        duration = skill.duration;
        StartCoroutine(DealDamageOverTime());
    }

    private void Update()
    {
        if (Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DealDamageOverTime()
    {
        while (Time.time - startTime < duration)
        {
            ApplyDamageToObjectsInRadius();
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void ApplyDamageToObjectsInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, skill.range, skill.affectedLayers);

        foreach (var hitCollider in hitColliders)
        {
            Health enemyHealth = hitCollider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(skill.damage);
            }
        }
    }

    public void Initialize(SkillSO skillSO)
    {
        skill = skillSO;
    }
}
