using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float cameraRotationSpeed = 2f;
    private bool rotLeft;
    private bool rotRight;
    private InputSystem inputSystem;
    private Rigidbody rigidbody;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Camera.CamRotLeft.started += i => rotLeft = true;
            inputSystem.Camera.CamRotRight.started += i => rotRight = true;
            inputSystem.Camera.CamRotLeft.canceled += i => rotLeft = false;
            inputSystem.Camera.CamRotRight.canceled += i => rotRight = false;
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCameraLeft();
        RotateCameraRight();
    }

    void RotateCameraLeft()
    {
        if (rotLeft)
        {
            float x = transform.eulerAngles.x;
            float y = transform.eulerAngles.y;
            rigidbody.rotation = Quaternion.Euler(new Vector3(x, y - (cameraRotationSpeed * Time.fixedDeltaTime)));
        }
        
    }

    void RotateCameraRight()
    {
        if (rotRight)
        {
            float x = transform.eulerAngles.x;
            float y = transform.eulerAngles.y;
            rigidbody.rotation = Quaternion.Euler(new Vector3(x, y + (cameraRotationSpeed * Time.fixedDeltaTime))); ;
        }
    }
}
