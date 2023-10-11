using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectsprojectiles : MonoBehaviour
{
    private Transform target;
    private float speed = 70f;
    private Vector3 pos;

    public void Seek(Transform _target)
    {
        target = _target;
        pos = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }


}
