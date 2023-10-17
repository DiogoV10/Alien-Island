using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform target;
    //public GameObject obj;
    [Header("Attributes")]

    [SerializeField] private EnemySO boss;
    [SerializeField] private List<Objectsprojectiles> objects;

    private float throwCountDown = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (throwCountDown <= 0f)
        {
            ChooseRandomProjectile();
            throwCountDown = 1f / boss.fireRate;
        }

        throwCountDown -= Time.deltaTime;
    }

    void LevitateAll()
    {
        //foreach (Objectsprojectiles obj in objects) StartCoroutine(obj.Levitate());
    }

    void ChooseRandomProjectile()
    {
        int random = Random.Range(0, objects.Count);
        //Objectsprojectiles objGO = obj.GetComponent<Objectsprojectiles>();
        if (objects != null) 
        {
            objects[random].Seek(target); 
        }  
    }

}
