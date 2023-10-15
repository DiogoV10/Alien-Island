using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectsprojectiles : MonoBehaviour
{
    private Transform target;
    private float speed = 70f;
    private Vector3 pos;
    private bool isLevitated = false;
    [SerializeField] private float levitateTime = 3f;

    public void Seek(Transform _target)
    {
        target = _target;
        pos = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        //transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        if(!isLevitated) StartCoroutine(Levitate());
    }

    IEnumerator Levitate()
    {
        float startTime = Time.time;
        while(Time.time < startTime + levitateTime)
        {
            transform.Translate(new Vector3(0, 0.5f, 0) * Time.deltaTime);
            isLevitated = true;
            yield return null;
        }
    }

}
