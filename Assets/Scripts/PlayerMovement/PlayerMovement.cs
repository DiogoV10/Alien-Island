using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float smoothTime = 1f;


    private bool isJumping = false;
    private bool isFalling = false;
    private bool isGround = false;
    private bool isRunning = false;
    private bool isWalking = false;
    private bool isDashing = false;

    private bool shouldFaceObject = false; // Initially, the player won't face the specific object 

    private bool inGame = false;

    private Vector2 velocity;

    private float horizontalVelocity = 0f;
    private float verticalVelocity = 0f;
    private float currentHorizontalVelocity = 0f;
    private float currentVerticalVelocity = 0f;
    private float lastGroundedTime = 0f;


    [SerializeField] private GameObject lockOnTarget;

    [Header("Dash")]
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;

    [Header("Jump")]
    [SerializeField] float jumpvalue = 1f;
    [SerializeField] float jumpGracePeriod = 1f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Slope")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mov = playerInput.actions.FindAction("Move");
        dash = playerInput.actions.FindAction("Dash");

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
        if (!inGame)
        {
            velocity = new Vector2(Mathf.SmoothDamp(velocity.x, 0, ref currentHorizontalVelocity, smoothTime), Mathf.SmoothDamp(velocity.y, 0, ref currentVerticalVelocity, smoothTime));
            return;
        }

        if (isRunning && PlayerCombat.Instance.CanMove() && PlayerCombat.Instance.CanRun()) Run();
        //else if (dash.WasPressedThisFrame()) StartCoroutine(Dashing());
        else if (PlayerCombat.Instance.CanMove()) MovePlayer();
    }

    private void Update()
    {
        if (!inGame)
        {
            Debug.Log(mov.ReadValue<Vector2>());
            velocity = new Vector2(Mathf.SmoothDamp(velocity.x, 0, ref currentHorizontalVelocity, smoothTime), Mathf.SmoothDamp(velocity.y, 0, ref currentVerticalVelocity, smoothTime));
            return;
        }

        lockOnTarget = LockOn.Instance.GetLockedEnemy();

        if (lockOnTarget == null)
        {
            shouldFaceObject = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFaceObject(!shouldFaceObject);
        }

        Vector2 direction = mov.ReadValue<Vector2>();

        if (direction == Vector2.zero)
        {
            isWalking = false;
            isRunning = false;
        }
        else
            isWalking = true;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);

        CalculateLocomotionAnimation();
        SpeedControl();

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            rigidBody.drag = 5;
        }
        else
        {
            rigidBody.drag = 0;
        }

        if (Time.time - lastGroundedTime <= jumpGracePeriod)
        {
            isGround = true;
            isFalling = false;
            exitingSlope = false;
        }
        else
        {
            isGround = false;
            isJumping = false;

            if ((isJumping && rigidBody.velocity.y < 0) || rigidBody.velocity.y < -2)
            {
                isFalling = true;
            }
        }

        rigidBody.useGravity = !OnSlope();
    }

    private void CalculateLocomotionAnimation()
    {
        if (shouldFaceObject && lockOnTarget != null)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;


            Vector3 movementDirection = SyncWithCameraRotation();

            Vector3 inputDirection = new Vector3(movementDirection.x, 0, movementDirection.z).normalized;

            if (isRunning && isWalking)
            {
                horizontalVelocity = Vector3.Dot(right, inputDirection) * runValue;
                verticalVelocity = Vector3.Dot(forward, inputDirection) * runValue;
            }
            else if (isWalking && !isRunning)
            {
                horizontalVelocity = Vector3.Dot(right, inputDirection) * speed;
                verticalVelocity = Vector3.Dot(forward, inputDirection) * speed;
            }
            else
            {
                horizontalVelocity = 0f;
                verticalVelocity = 0f;
            }

            velocity = new Vector2(Mathf.SmoothDamp(velocity.x, horizontalVelocity, ref currentHorizontalVelocity, smoothTime), Mathf.SmoothDamp(velocity.y, verticalVelocity, ref currentVerticalVelocity, smoothTime));
        }
        else
        {
            if (isRunning && isWalking)
            {
                velocity = new Vector2(0, Mathf.SmoothDamp(velocity.y, 20, ref verticalVelocity, smoothTime));
            }
            else if (isWalking && !isRunning)
            {
                velocity = new Vector2(0, Mathf.SmoothDamp(velocity.y, 10, ref verticalVelocity, smoothTime));
            }
            else
            {
                velocity = new Vector2(Mathf.SmoothDamp(velocity.x, 0, ref horizontalVelocity, smoothTime), Mathf.SmoothDamp(velocity.y, 0, ref verticalVelocity, smoothTime));
            }
        }
    }

    void MovePlayer()
    {
        Vector3 movementDirection = SyncWithCameraRotation();

        if (OnSlope() && !exitingSlope)
        {
            rigidBody.AddForce(GetSlopeMoveDirection(movementDirection) * speed * 20f, ForceMode.Force);

            if (rigidBody.velocity.y > 0)
            {
                rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        rigidBody.AddForce(movementDirection * speed * 10f, ForceMode.Force);
    }

    void Run()
    {
        Vector3 movementDirection = SyncWithCameraRotation();

        if (OnSlope() && !exitingSlope)
        {
            rigidBody.AddForce(GetSlopeMoveDirection(movementDirection) * runValue * 20f, ForceMode.Force);

            if (rigidBody.velocity.y > 0)
            {
                rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        rigidBody.AddForce(movementDirection * runValue * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rigidBody.velocity.magnitude > speed)
                rigidBody.velocity = rigidBody.velocity.normalized * speed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

            if (isRunning)
            {
                if (flatVel.magnitude > runValue)
                {
                    Vector3 velocity = flatVel.normalized * runValue;

                    rigidBody.velocity = new Vector3(velocity.x, rigidBody.velocity.y, velocity.z);
                }
            }
            else
            {
                if (flatVel.magnitude > speed)
                {
                    Vector3 velocity = flatVel.normalized * speed;

                    rigidBody.velocity = new Vector3(velocity.x, rigidBody.velocity.y, velocity.z);
                }
            }
        }
    }

    void Jump()
    {
        if (!isGrounded || !PlayerCombat.Instance.CanJump()) return;

        exitingSlope = true;
        isJumping = true;

        lastGroundedTime = 0;

        rigidBody.AddForce(Vector3.up * jumpvalue, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, transform.position.y - groundCheck.position.y))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 movementDirection)
    {
        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
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
            rigidBody.velocity = movementDirection * dashSpeed;
            //rigidBody.MovePosition(transform.position + movementDirection * Time.deltaTime * dashSpeed);
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

            // Set the rotation to face the specific object
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

    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    public bool IsGround()
    {
        return isGround;
    }

    public Vector2 Velocity()
    {
        return velocity;
    }
}
