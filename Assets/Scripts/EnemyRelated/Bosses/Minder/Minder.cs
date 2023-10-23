using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minder : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Attributes")]
    [SerializeField] private float bossHealth;
    [SerializeField] private EnemySO boss;

    [Header("Leviate Attack")]
    [SerializeField] private List<LevitateAttack> levitateObjects;
    [SerializeField] private float timeToNextAttack = 3f;
    [SerializeField] private float throwCountdown = 0f;
    
    [Header("Bombing Attack")]
    [SerializeField] private List<ObjectBombingAttack> bombingObjects;

    private bool objectThrowAttackOn = false, isAllLevitated = false, objectBombingAttackOn = false;
    public UnityAction colliderWithGround;
    public bool ObjectBombingAttackOn => objectBombingAttackOn;

    //[Header("Melee Attack")]


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
            throwCountdown = 1f / boss.fireRate;
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
                int random = 1;
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

}
