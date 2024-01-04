using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venous : MonoBehaviour
{
    VenomCloudAttack cloudAttack;
    bool cloudAttackOn = false, projectileAttackOn = false;
    [SerializeField] float timeToNextAttack = 3f, projectileAttackDuration = 20f, venomSpawnRate;
    [SerializeField] GameObject venomSpit;
    [SerializeField] Transform target;
    Animator animator;

    [Header("VenousSO reference")]
    [SerializeField] VenousSO venousSO;

    private EnemyAudio enemyAudio;

    // Start is called before the first frame update
    void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();
        cloudAttack = GetComponent<VenomCloudAttack>();
        venomSpawnRate = 1f / venousSO.fireRate;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChooseAttack();
        if (cloudAttackOn || cloudAttack.venousEffectTime > .1f)
        {
            if (cloudAttack.attackTime < .1f)
            {
                cloudAttackOn = false;
                animator.SetBool("isSmokeAttack", false);
            }
            cloudAttack.StartAttack();
        }

        if (projectileAttackOn)
        {
            ProjectileAttack();
        }
            
    }

    void ProjectileAttack()
    {
        projectileAttackDuration -= Time.deltaTime;
        venomSpawnRate -= Time.deltaTime;
        if (projectileAttackDuration > .1f)
        {
            if(venomSpawnRate < .1f)
            {
                GameObject go = Instantiate(venomSpit, transform.position, Quaternion.identity);
                go.GetComponent<VenomProjectile>().SetTarget(target);

                venomSpawnRate = 1f / venousSO.fireRate;
            }
        }
        else
        {
            projectileAttackOn = false;
            animator.SetBool("isSpitAttack", false);
            projectileAttackDuration = 20f;
            venomSpawnRate = 2f;
        }
    }

    void ChooseAttack()
    {
        if (!cloudAttackOn && !projectileAttackOn)
        {
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = 1;
                switch (random)
                {
                    case 0:
                        cloudAttackOn = true;
                        animator.SetBool("isSmokeAttack", true);
                        timeToNextAttack = 3f;
                        break;

                    case 1:
                        projectileAttackOn = true;
                        timeToNextAttack = 3f;
                        animator.SetBool("isSpitAttack", true);
                        break;
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        venousSO.hp -= damage;
        if(enemyAudio) enemyAudio.PlayHurtSound();
        Debug.Log(venousSO.hp);
        if (venousSO.hp <= 0f)
        {
            MainQuests.Instance.QuestEnd();
            SkillPoints.Instance.IncreaseSkillPoints();
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        //callback(gameObject);
        Destroy(gameObject);
    }
}
