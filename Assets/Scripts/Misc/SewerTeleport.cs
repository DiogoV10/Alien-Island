using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerTeleport : MonoBehaviour
{
    [SerializeField] private Transform exitWaypoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("hello");

            other.GetComponent<PlayerMovement>().enabled = false;
            other.GetComponent<Rigidbody>().MovePosition(exitWaypoint.position);
            StartCoroutine(EnablePlayerMovement(other.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator EnablePlayerMovement(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(0.1f);
        playerMovement.enabled = true;
    }

}
