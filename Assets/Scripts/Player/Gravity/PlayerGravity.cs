using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{


    public static PlayerGravity Instance { get; private set; }


    private float gravity = 9.8f;
    private float canUseGravityTimer = 0f;
    private bool canUseGravity = true;
    private bool gravityOn = true;
    private Rigidbody rigidbody;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!PlayerMovement.Instance.IsGrounded() && canUseGravity && gravityOn)
        {
            rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

        if (canUseGravityTimer >= 1.5f && !canUseGravity)
        {
            EnableCanUseGravity();
        }
        else
        {
            canUseGravityTimer += Time.deltaTime;
        }
    }

    public void AddForceOnAirAttack(float force)
    {
        DisableCanUseGravity();
        rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        Invoke(nameof(EnableCanUseGravity), 1f);
    }

    private void EnableCanUseGravity()
    {
        canUseGravity = true;
    }

    public void DisableCanUseGravity()
    {
        canUseGravity = false;
        rigidbody.velocity = Vector3.zero;

        canUseGravityTimer = 0f;
    }

    public void SetGravityOnOff(bool choice)
    {
        gravityOn = choice;
    }
}
