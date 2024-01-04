using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerEntrance : MonoBehaviour
{
    private Animator SewerEntranceAnimator;
    private Animator SewerEntranceAnimator2;

    private void Awake()
    {
        SewerEntranceAnimator = GetComponent<Animator>();
        SewerEntranceAnimator2 = GetComponent<Animator>();
    }

private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SewerEntranceAnimator.enabled = true;
            SewerEntranceAnimator2.enabled = true;
        }
    }

}
