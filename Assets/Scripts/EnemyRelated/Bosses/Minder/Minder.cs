using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minder : MonoBehaviour, IEntity
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Attributes")]
    [SerializeField] MinderSO minderSO;

    [Header("Leviate Attack")]
    [SerializeField] private List<LevitateAttack> levitateObjects;
    [SerializeField] private float timeToNextAttack = 3f;
    [SerializeField] private float throwCountdown = 0f;
    
    [Header("Bombing Attack")]
    [SerializeField] private List<ObjectBombingAttack> bombingObjects;

    private bool objectThrowAttackOn = false, isAllLevitated = false, objectBombingAttackOn = false;
    public UnityAction colliderWithGround;
    public bool ObjectBombingAttackOn => objectBombingAttackOn;
    public bool ObjectThrowAttackOn => objectThrowAttackOn;



    // Start is called before the first frame update
    void Start()
    {
        colliderWithGround += ObjectOnFloor;
    }

    // Update is called once per frame
    void Update()
    {
        ChooseAttack();
        if (objectThrowAttackOn) ObjectThrowAttack();
        else if(objectBombingAttackOn) ObjectBommbingAttack();
        Debug.Log(objectBombingAttackOn);
    }

    void LevitateAll()
    {
        foreach (LevitateAttack obj in levitateObjects)
        {
            obj.StartLevitate();
        }
    }

    void ChooseRandomObject()
    {
        int random = Random.Range(0, levitateObjects.Count);
        if (levitateObjects != null) 
        {
            levitateObjects[random].SetTarget(target);
            levitateObjects[random].Throw();
        }  
    }

    void ObjectThrowAttack()
    {

        if (!isAllLevitated)
        {
            LevitateAll();
            isAllLevitated = true;
        }

        else if (throwCountdown <= 0f)
        {
            ChooseRandomObject();
            throwCountdown = 1f / minderSO.fireRate;
        }

        throwCountdown -= Time.deltaTime;

        if (levitateObjects[0].attackDuration <= 0.1f)
        {
            objectThrowAttackOn = false;
            isAllLevitated = false;
        }
    }

    void ObjectBommbingAttack()
    {
        foreach (ObjectBombingAttack obj in bombingObjects) 
        {
            obj.GoToPositionCoroutine(this);
        } 
    }

    void ObjectOnFloor()
    {
        objectBombingAttackOn = false;
    }

    void ChooseAttack()
    {
        if (!objectThrowAttackOn && !objectBombingAttackOn)
        {
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        objectThrowAttackOn = true;
                        foreach (LevitateAttack obj in levitateObjects) obj.attackDuration = 20f;
                        timeToNextAttack = 3f;
                        break;

                    case 1:
                        objectBombingAttackOn = true;
                        timeToNextAttack = 3f;
                        break;
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        minderSO.hp -= damage;
        Debug.Log(minderSO.hp);
        if (minderSO.hp <= 0f) Die();
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        foreach (LevitateAttack obj in levitateObjects)
        {
            obj.collider.isTrigger = false;
        }
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        //callback(gameObject);
        Destroy(gameObject);
    }
}
