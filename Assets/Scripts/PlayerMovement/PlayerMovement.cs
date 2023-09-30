using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInput playerInput;
    private InputAction mov, run, jump;
    private Rigidbody rigidBody;
    [SerializeField] float runValue = 20f;
    [SerializeField] float speed = 5f;

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
        run = playerInput.actions.FindAction("Run");
        jump = playerInput.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrouded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);
        if (run.IsPressed()) StartCoroutine(Run());
        else if (jump.IsPressed() && isGrouded) Jump();
        else MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 direction = mov.ReadValue<Vector2>();
        Vector3 transfDirection = transform.TransformDirection(new Vector3(direction.x, 0, direction.y));
        rigidBody.MovePosition(transform.position + transfDirection * Time.deltaTime * speed);
    }

    IEnumerator Run()
    {
        Vector2 direction = mov.ReadValue<Vector2>();
        Vector3 transfDirection = transform.TransformDirection(new Vector3(direction.x, 0, direction.y));
        rigidBody.MovePosition(transform.position + transfDirection * runValue * Time.deltaTime);
        yield return null;
        
    }

    void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpvalue, ForceMode.Impulse);
    }
}
