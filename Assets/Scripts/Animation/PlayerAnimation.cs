using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{


    private const string IS_RUNNING = "IsRunning";


    [SerializeField] private PlayerMovement playerMovement;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, playerMovement.IsWalking());
    }


}
