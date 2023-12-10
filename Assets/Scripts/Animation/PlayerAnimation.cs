using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{


    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_FALLING = "IsFalling";
    private const string IS_LANDING = "IsLanding";
    private const string IS_ATTACKING = "IsAttacking";
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    

    private Animator animator;
    bool inGame;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
       

        animator.SetBool(IS_WALKING, PlayerMovement.Instance.IsWalking());
        animator.SetBool(IS_RUNNING, PlayerMovement.Instance.IsRunning());
        animator.SetBool(IS_JUMPING, PlayerMovement.Instance.IsJumping());
        animator.SetBool(IS_FALLING, PlayerMovement.Instance.IsFalling());
        animator.SetBool(IS_LANDING, PlayerMovement.Instance.IsLanding());
        animator.SetBool(IS_ATTACKING, PlayerCombat.Instance.IsAttacking());

        animator.SetFloat(HORIZONTAL, PlayerMovement.Instance.Velocity().x);
        animator.SetFloat(VERTICAL, PlayerMovement.Instance.Velocity().y);
    }


}
