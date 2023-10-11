using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform target;
    public GameObject obj;

    [Header("Attributes")]
    [SerializeField] EnemySO boss;
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
            Throw();
            throwCountDown = 1f / boss.fireRate;
        }

        throwCountDown -= Time.deltaTime;
    }

    void Throw()
    {
        Objectsprojectiles objGO = obj.GetComponent<Objectsprojectiles>();
        if (objGO != null) objGO.Seek(target);
    }

}
