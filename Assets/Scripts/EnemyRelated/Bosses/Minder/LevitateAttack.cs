using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateAttack : MonoBehaviour
{
    private Transform target;
    private Rigidbody rigidbody;
    
    [Header("Attack")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float levitateTime = 3f;
    [SerializeField] public float attackDuration = 20f;
    private bool isLevitated = false, stopedLevitate = false;

    private Vector3 pos;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLevitated) attackDuration -= Time.deltaTime;
        if (target == null) return;
        else if (attackDuration <= 0f)
        {
            DropObject();
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        pos = target.position;
    }

    public IEnumerator LevitateObject()
    {
        rigidbody.useGravity = false;
        float startTime = Time.time;
        stopedLevitate = true;
        while (Time.time < startTime + levitateTime)
        {
            transform.Translate(new Vector3(0, 0.5f, 0) * Time.deltaTime);
            yield return null;
        }
        isLevitated = true;
    }

    void DropObject()
    {
        rigidbody.useGravity = true;
        attackDuration = 20f;
        isLevitated = false;
        stopedLevitate = false;
    }

    public void Throw()
    {
        if (isLevitated && rigidbody.velocity.magnitude == 0 && attackDuration > 0)
        {
            Vector3 throwVector = new Vector3((pos.x - transform.position.x), 0, (pos.z - transform.position.z)).normalized * speed;
            rigidbody.AddForce(throwVector, ForceMode.Impulse);
        }
    }

    public void StartLevitate()
    {
        if (!isLevitated && !stopedLevitate)
        {
            StartCoroutine(LevitateObject());
        }
    }
}
