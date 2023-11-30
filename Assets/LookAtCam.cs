using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(_mainCam.transform.position);
    }
}
