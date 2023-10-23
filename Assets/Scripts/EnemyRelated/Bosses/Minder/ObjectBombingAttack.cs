using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectBombingAttack : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField] private Transform target;
    private Minder minder;

    private bool inPosition = false;
    private float goToPositionTime = 0.25f;
    [SerializeField] private float waitUntilBombing = 1.5f;
    private int groundLayer = 6;
    private float downForce = 2f, objectSeparateForce = 30f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inPosition)
        {
            waitUntilBombing -= Time.deltaTime;
            FollowPlayer();
            if (waitUntilBombing <= 0)
            {
                Bombing();
            }
        }
        //if(!inPosition) GoToPosition();
    }

    IEnumerator GoToPosition(Minder _minder) //this will be a coroutine
    {
        float startTime = Time.time;
        Vector3 objectpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        while (Time.time < startTime + goToPositionTime)
        {
            Vector3 targetpos = new Vector3(target.position.x, 10, target.position.z);
            float distMagnitude = (targetpos - objectpos).magnitude;
            //transform.Translate((targetpos - objectpos).normalized * Time.fixedDeltaTime);
            rigidBody.MovePosition(transform.position + ((targetpos - objectpos).normalized * (distMagnitude / goToPositionTime)) * Time.fixedDeltaTime);
            yield return null;
        }
        rigidBody.useGravity = false;
        inPosition = true;
        minder = _minder;
    }

    void FollowPlayer()
    {

        if (waitUntilBombing > 0f) rigidBody.MovePosition(new Vector3(target.position.x, 10, target.position.z));
    }

    void Bombing()
    {
        rigidBody.AddForce(Vector3.down * downForce, ForceMode.Impulse);
        if (waitUntilBombing <= 0f)
        {
            rigidBody.useGravity = true;
        }

    }

    public void GoToPositionCoroutine(Minder _minder)
    {
        if (!inPosition)
        {
            StartCoroutine(GoToPosition(_minder));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            //Debug.Log("colidiu");
            if(minder!= null)
            {
                minder.colliderWithGround.Invoke();
            }
            inPosition = false;
            waitUntilBombing = 1.5f;
            Vector3 vector = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            rigidBody.AddForce(vector * objectSeparateForce, ForceMode.Impulse);
        }
    }

    public float WaitUntilBombing()
    {
        return waitUntilBombing;
    }

}
