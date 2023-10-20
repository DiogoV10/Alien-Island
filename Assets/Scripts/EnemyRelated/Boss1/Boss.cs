using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEntity
{
    [SerializeField] private Transform target;
    //public GameObject obj;
    [Header("Attributes")]

    [SerializeField] private EnemySO boss;
    [SerializeField] private List<Objectsprojectiles> objects;
    //[SerializeField] private List<int> attackList;
    [SerializeField] private float timeToNextAttack = 3f;

    private float throwCountdown = 0f, levitateAttackTime, meleeAttackTime;

    private bool objectAttackOn = false, isAllLevitated = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChooseAttack();
        if(objectAttackOn) ObjectAttack();
    }

    void LevitateAll()
    {
        foreach (Objectsprojectiles obj in objects)
        {
            obj.StartLevitate();
        }
    }

    void ChooseRandomProjectile()
    {
        int random = Random.Range(0, objects.Count);
        if (objects != null) 
        {
            objects[random].SetTarget(target);
        }  
    }

    void ObjectAttack()
    {
        if (!isAllLevitated)
        {
            LevitateAll();
            isAllLevitated = true;
        }

        else if (throwCountdown <= 0f)
        {
            ChooseRandomProjectile();
            throwCountdown = 1f / boss.fireRate;
        }

        throwCountdown -= Time.deltaTime;

        if (objects[0].attackDuration <= 0.1f)
        {
            objectAttackOn = false;
            isAllLevitated = false;
        }
    }

    void ChooseAttack()
    {
        //Debug.Log(objectAttackOn);
        if (!objectAttackOn)
        {
            //Debug.Log(objectAttackOn);
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        objectAttackOn = true;
                        foreach (Objectsprojectiles obj in objects) obj.attackDuration = 20f;
                        timeToNextAttack = 3f;
                        break;

                    case 1:
                        timeToNextAttack = 3f;
                        break;
                }
                //Debug.Log(objectAttackOn);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        boss.enemyHP -= damage;
        Debug.Log(boss.enemyHP);
        if (boss.enemyHP <= 0f) Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        Debug.Log("Boss Died!");
    }
}
