using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Dash : MonoBehaviour
{
    private InputSystem inputSystem;
    private InputAction mov;
    private PlayerInput playerInput;
    private Rigidbody rigidBody;
    [SerializeField] private Camera follow;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mov = playerInput.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Movement.Dash.performed += i => StartCoroutine(Dashing());
            //inputSystem.Movement.Dash.canceled += i => StartCoroutine(Dashing());
            //Debug.Log(inputSystem.Movement.Dash);
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }


    // Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator Dashing()
    {
        float startTime = Time.time;
        Vector3 movementDirection = SyncWithCameraRotation();
        Debug.Log("1");

        while (Time.time < startTime + dashTime)
        {
            Debug.Log("2");
            rigidBody.velocity = movementDirection * dashSpeed;
            Debug.Log(rigidBody.velocity);
            yield return null;
        }
    }

    Vector3 SyncWithCameraRotation()
    {
        Vector2 direction = mov.ReadValue<Vector2>();
        Vector3 movementInput = Quaternion.Euler(0, follow.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        Vector3 movementDirection = movementInput.normalized;
        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        return movementDirection;
    }
}
