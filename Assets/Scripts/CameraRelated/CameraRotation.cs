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
    private bool inGame = false;
    private InputSystem inputSystem;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 36f;
 
    void Start()
    {
        //transform.rotation = Quaternion.LookRotation(target.position - transform.position).normalized;
    }

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameState state)
    {
        inGame = state == GameState.InGame;
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
        if (!inGame) return;
        RotateCameraLeft();
        RotateCameraRight();
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

    void RotateCameraLeft()
    {
        if (rotLeft)
        {
            float x = transform.eulerAngles.x;
            float y = transform.eulerAngles.y;
            transform.RotateAround(target.position, Vector3.up, cameraRotationSpeed * Time.deltaTime);
        }
        
    }

    void RotateCameraRight()
    {
        if (rotRight)
        {
            float x = transform.eulerAngles.x;
            float y = transform.eulerAngles.y;
            transform.RotateAround(target.position, Vector3.down, cameraRotationSpeed * Time.deltaTime);
        }
    }
}
