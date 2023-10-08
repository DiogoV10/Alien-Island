using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput playerInput;
    private InputAction mov, dash;
    private InputSystem inputSystem;
    private Rigidbody rigidBody;
    [SerializeField] private Camera follow;
    [SerializeField] private float runValue = 20f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isRunning = false;
    private bool isDashing = false;

    [Header("Dash")]
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;

    [Header("Jump")]
    [SerializeField] float jumpvalue = 1f;
    [SerializeField] private bool isGrouded = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mov = playerInput.actions.FindAction("Move");
        dash = playerInput.actions.FindAction("Dash");
    }

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Movement.Jump.performed += i => Jump(); 
            inputSystem.Movement.Run.performed += i => isRunning = !isRunning; 
            inputSystem.Movement.Dash.performed += i => StartCoroutine(Dashing()); 
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        isGrouded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);
        if (isRunning) Run();
        //else if (dash.WasPressedThisFrame()) StartCoroutine(Dashing());
        else if(isDashing == false) MovePlayer();
        //Debug.Log(rigidBody.velocity.y);


    }

    void MovePlayer()
    {
        Vector3 movementDirection = SyncWithCameraRotation();
        rigidBody.MovePosition(transform.position + movementDirection * Time.deltaTime * speed);

    }

    void Run()
    {
        Vector3 movementDirection = SyncWithCameraRotation();
        rigidBody.MovePosition(transform.position + movementDirection * Time.deltaTime * runValue);

    }

    void Jump()
    {
        if (!isGrouded) return;
        rigidBody.AddForce(Vector3.up * jumpvalue, ForceMode.Impulse);
    }

    IEnumerator Dashing()
    {
        isDashing = true;
        float startTime = Time.time;
        Vector3 movementDirection = SyncWithCameraRotation();

        while (Time.time < startTime + dashTime)
        {
            rigidBody.MovePosition(transform.position + movementDirection * Time.deltaTime * dashSpeed); ;
            yield return null;
            isDashing = false;
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
