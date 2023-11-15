using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] Transform camera;
    Vector3 dir;

    private void Update()
    {
        dir.z = camera.eulerAngles.y;
        transform.eulerAngles = dir;
    }
}
