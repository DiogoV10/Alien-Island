using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Dash : MonoBehaviour
{
    private InputSystem inputSystem;
    private Rigidbody rigidBody;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
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
    void Update()
    {
        
    }

    IEnumerator Dashing()
    {
        float startTime = Time.time;
        Debug.Log("1");

        while(Time.time < startTime + dashTime)
        {
            rigidBody.MovePosition(transform.position * dashSpeed * Time.fixedDeltaTime);
            Debug.Log("2");
            yield return null;
        }
    }
}
