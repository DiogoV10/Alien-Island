using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    float gravity = 9.8f;
    Rigidbody rigidbody;
    PlayerMovement isGrouded;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        isGrouded = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrouded.IsGrounded())
        {
            Debug.Log("A tentar usar Gravidade");
            rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }
}
