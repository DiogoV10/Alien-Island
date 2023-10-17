using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectsprojectiles : MonoBehaviour
{
    private Transform target;
    private Rigidbody rigidbody;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float levitateTime = 3f;

    private Vector3 pos;
    private bool isLevitated = false, stopedLevitate = false;
    

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (!isLevitated && !stopedLevitate)
        {
            StartCoroutine(Levitate());
        }
        else if (isLevitated && rigidbody.velocity.magnitude == 0 && Time.time <= 20f)
        {
            Throw();
        }
        else if (Time.time > 20f) DeLevitate();
        Debug.Log(Time.time);
    }

    public void Seek(Transform _target)
    {
        target = _target;
        pos = target.position;
    }

    public IEnumerator Levitate()
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

    void DeLevitate()
    {
        rigidbody.useGravity = true;
        //timeCount = Time.time;
    }

    void Throw()
    {
        Vector3 throwVector = new Vector3((pos.x - transform.position.x), 0, (pos.z - transform.position.z)).normalized * speed;
        rigidbody.AddForce(throwVector, ForceMode.Impulse);
    }
}
