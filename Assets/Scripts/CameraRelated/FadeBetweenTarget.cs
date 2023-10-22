using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBetweenTarget : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] LayerMask fadeableLayers;

    private void FixedUpdate()
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position, GetDirection(), GetDistance());
        if (hit.Length > 0)
        {
            FadeObject(hit[0]);
        }
    }

    private void FadeObject(RaycastHit objectToHide)
    {
        objectToHide.transform.GetComponentInChildren<Renderer>().materials[0].SetFloat("_Opacity", .2f);
    }

    private float GetDistance()
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    private Vector3 GetDirection()
    {
        return (target.transform.position - transform.position).normalized;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GetDirection() * GetDistance());
    }
}
