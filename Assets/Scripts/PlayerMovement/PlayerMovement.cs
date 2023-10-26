using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    public static PlayerMovement Instance { get; private set; }


    // Start is called before the first frame update
    private PlayerInput playerInput;
    private InputAction mov, dash;
    private InputSystem inputSystem;
    private Rigidbody rigidBody;
    [SerializeField] private Camera follow;
    [SerializeField] private float runValue = 20f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float lockRotationSpeed = 25f;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isLanding = false;
    private bool isRunning = false;
    private bool isWalking = false;
    private bool isDashing = false;

    public bool shouldFaceObject = false; // Initially, the player won't face the specific object 


    [SerializeField] private GameObject lockOnTarget;

    [Header("Dash")]
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;

    [Header("Jump")]
    [SerializeField] float jumpvalue = 1f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;


    void Awake()
    {
        Instance = this;

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
            //inputSystem.Movement.Dash.performed += i => StartCoroutine(Dashing()); 
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);
        if (isRunning && PlayerCombat.Instance.CanMove() && PlayerCombat.Instance.CanRun()) Run();
        //else if (dash.WasPressedThisFrame()) StartCoroutine(Dashing());
        else if (PlayerCombat.Instance.CanMove()) MovePlayer();

        if (isGrounded)
        {
            if (isFalling)
            {
                isLanding = true;
            }
            isJumping = false;
            isFalling = false;
        }
        else
        {
            if (!isJumping)
            {
                isFalling = true;
            }
            isLanding = false;
        }
    }

    private void Update()
    {
        Vector2 direction = mov.ReadValue<Vector2>();

        lockOnTarget = LockOn.Instance.GetLockedEnemy();

        if (lockOnTarget == null)
        {
            shouldFaceObject = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFaceObject(!shouldFaceObject);
        }

        if (direction == Vector2.zero)
        {
            isWalking = false;
            isRunning = false;
        }
        else
            isWalking = true;
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
        if (!isGrounded || !PlayerCombat.Instance.CanJump()) return;
        isJumping = true;
        isFalling = false;
        isLanding = false;
        rigidBody.AddForce(Vector3.up * jumpvalue, ForceMode.Impulse);
    }

    public void AddForceOnAirAttack(float forceValue)
    {
        //rigidBody.useGravity = false;
        rigidBody.AddForce(Vector3.up * forceValue, ForceMode.Impulse);
        //Invoke("EnableGravity", 0.5f);
    }

    private void EnableGravity()
    {
        rigidBody.useGravity = true;
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
        Vector3 movementInput = Vector3.zero;
        Vector3 movementDirection = movementInput.normalized;

        if (shouldFaceObject && lockOnTarget != null)
        {
             // Calculate the direction from the player to the specific object
            Vector3 objectDirection = lockOnTarget.transform.position - transform.position;
            objectDirection.y = 0f; // Ignore the vertical component
            objectDirection.Normalize();

            // Set the rotation of the rigidBody to face the specific object
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectDirection, Vector3.up), lockRotationSpeed * Time.deltaTime);

            // Continue moving in the direction of the input
            movementInput = Quaternion.Euler(0, follow.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

            movementDirection = movementInput.normalized;
        }
        else
        {
            // Calculate direction relative to the camera
            movementInput = Quaternion.Euler(0, follow.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

            movementDirection = movementInput.normalized;

            if (movementDirection != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
            }
        }

        return movementDirection;
    }

    public void RotatePlayerTowardsInput()
    {
        Vector2 direction = mov.ReadValue<Vector2>();
        Vector3 directionInput = Quaternion.Euler(0, follow.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

        if (direction != Vector2.zero && !shouldFaceObject)
        {
            if (shouldFaceObject)
            {
                //Vector3 objectDirection = lockOnTarget.transform.position - transform.position;
                ////objectDirection.y = 0f;
                ////objectDirection.Normalize();
                //Vector3 objectDir = Quaternion.Euler(0, follow.transform.eulerAngles.y, 0) * new Vector3(objectDirection.x, 0, objectDirection.y);

                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectDir, Vector3.up), 500 * Time.deltaTime);
            }
            else
            {
                Quaternion desiredRotation = Quaternion.LookRotation(directionInput, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 500 * Time.deltaTime);
            }
        }
    }

    public void ToggleFaceObject(bool shouldFace)
    {
        if (lockOnTarget != null)
            shouldFaceObject = shouldFace;
        else
            shouldFaceObject = false;
    }

    public bool ShouldFaceObject()
    {
        return shouldFaceObject;
    }

    public bool IsRunning()
    {
        if (PlayerCombat.Instance.CanRun())
            return isRunning;
        else
            return false;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsFalling()
    {
        return isFalling;
    }

    public bool IsLanding()
    {
        return isLanding;
    }

    public bool IsGrounded()
    {
        return isGrounded; 
    }
}
