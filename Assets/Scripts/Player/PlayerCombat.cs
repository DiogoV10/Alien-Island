// Ignore Spelling: Untoggle

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using V10;

public class PlayerCombat : MonoBehaviour
{


    public static PlayerCombat Instance { get; private set; }


    [System.Serializable]
    public class ComboTransition
    {
        public ComboSO sourceCombo;
        public ComboSO targetCombo;
        public ComboCondition transitionCondition;
        public int attackIndex; // Index of the attack in the source combo when the transition can happen.
        public string sourceWeapon; // Source weapon name.
        public string targetWeapon; // Target weapon name.
    }

    public enum ComboCondition
    {
        None,
        Wait,
        Hold,
    }


    [SerializeField] private List<ComboTransition> comboTransitions;
    [SerializeField] private List<ComboSO> availableCombos;
    //[SerializeField] Weapon weapon;

    private Animator animator;


    private ComboSO currentCombo;
    private ComboSO nextCombo;


    private int comboCounter;
    private int clipNumber = 1;


    private bool canShoot = true;
    
    
    private bool canMove = true;
    private bool canRun = true;
    private bool canJump = true;
    private bool canAttack = true;
    private bool nextAttack = true;
    private bool buttonPressed = false;
    private bool isAttacking = false;

    private bool[] animationActive = new bool[2];


    private AnimatorOverrideController animatorOverrideController;


    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameInput.Instance.OnAttackMeleeAction += GameInput_OnAttackMeleeAction;
        GameInput.Instance.OnAttackMeleeHoldAction += GameInput_OnAttackMeleeHoldAction;
        GameInput.Instance.OnAttackRangeAction += GameInput_OnAttackRangeAction;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        ChangeRigWeight.Instance.SetRigWeight(0f);

        currentCombo = availableCombos[0];
    }

    private void GameInput_OnAttackRangeAction(object sender, System.EventArgs e)
    {
        if (canAttack && canShoot)
        {
            ChangeRigWeight.Instance.SetRigWeight(1f);

            animator.SetTrigger("Shoot");
            WeaponSelector.Instance.ChangeSystem(1);

            PlayerMovement.Instance.ToggleFaceObject(true);

            buttonPressed = false;

            InterruptCombo();

            canShoot = false;
            canRun = false;
        }
    }

    private void GameInput_OnAttackMeleeHoldAction(object sender, System.EventArgs e)
    {
        if (canAttack)
        {
            WeaponSelector.Instance.ChangeSystem(0);

            if (CheckComboTransitions(ComboCondition.Hold))
            {
                buttonPressed = true;
                canAttack = false;
            }
        }
    }

    private void GameInput_OnAttackMeleeAction(object sender, System.EventArgs e)
    {
        if (canAttack)
        {
            WeaponSelector.Instance.ChangeSystem(0);
            ChangeRigWeight.Instance.SetRigWeight(0f);

            CheckComboTransitions(ComboCondition.None);

            buttonPressed = true;
            isAttacking = true;
            canMove = false;
            canRun = false;
            canJump = false;
            canAttack = false;
        }
    }

    private void Update()
    {
        if (nextAttack && buttonPressed)
        {
            Attack();
        }

        if ((PlayerMovement.Instance.IsRunning() || PlayerMovement.Instance.IsWalking()) && canMove)
        {
            InterruptCombo();
        }

        animator.SetBool("IsAttacking", isAttacking);

        Debug.Log(comboCounter);
    }

    void Attack()
    {
        if (clipNumber > 2)
            clipNumber = 1;

        animationActive[clipNumber - 1] = !animationActive[clipNumber - 1];

        animatorOverrideController["Attack" + clipNumber] = currentCombo.combo[comboCounter].animationClip;

        animator.SetTrigger("Attack" + clipNumber);

        if (PlayerMovement.Instance.IsFalling())
        {
            PlayerMovement.Instance.AddForceOnAirAttack(2f);
        }

        nextAttack = false;
        buttonPressed = false;
        clipNumber++;

        PlayerMovement.Instance.RotatePlayerTowardsInput();
    }

    private bool CheckComboTransitions(ComboCondition condition)
    {
        //string currentWeapon = WeaponSelector.Instance.GetCurrentWeaponName();

        foreach (ComboTransition transition in comboTransitions)
        {
            if (transition.sourceCombo == currentCombo &&
                /*transition.sourceWeapon == currentWeapon &&*/
                comboCounter - 1 == transition.attackIndex &&
                transition.transitionCondition == condition)
            {
                currentCombo = transition.targetCombo;
                comboCounter = 0;
                return true;
            }
        }

        return false;
    }

    public void NextAttack()
    { 
        nextAttack = true;
        canJump = true;
    }

    public void ChangeCombo()
    {
        if (animationActive[0] != animationActive[1] || buttonPressed)
        {
            return;
        }

        CheckComboTransitions(ComboCondition.Wait);
    }

    public void ResetCombo ()
    {
        nextCombo = availableCombos[0];
        comboCounter = 0;
        currentCombo = nextCombo;
    }

    public void IncrementCombo()
    {
        comboCounter++;

        animationActive[0] = true; 
        animationActive[1] = true;
    }

    public void CanMoveAndRun()
    {
        if (animationActive[0] != animationActive[1] || buttonPressed) 
        {
            animationActive[0] = true;
            animationActive[1] = true;
            return;
        }

        canMove = true;
        canRun = true;
    }

    public void CanAttack()
    {
        canAttack = true;
    }

    public void CannotAttack()
    {
        canAttack = false;
    }

    public void CanShoot()
    {
        canShoot = true;
    }

    public void Shoot()
    {
        RangedWeapon.Instance.Shoot();
    }

    public void UntoggleLockOn()
    {
        PlayerMovement.Instance.ToggleFaceObject(false);
        canRun = true;
        ChangeRigWeight.Instance.SetRigWeight(0f);
    }

    public void InterruptCombo()
    {
        if (animationActive[0] != animationActive[1] || buttonPressed)
        {
            animationActive[0] = true;
            animationActive[1] = true;
            return;
        }

        canAttack = true;
        buttonPressed = false;
        nextAttack = true;
        canJump = true;
        canMove = true;
        canRun = true;
        isAttacking = false;

        animationActive[0] = true;
        animationActive[1] = true;

        nextCombo = availableCombos[0];
        comboCounter = 0;
        currentCombo = nextCombo;
    }

    public void CanHit()
    {
        MeleeWeapon.Instance.SetCanHit(true);
    }

    public void CannotHit()
    {
        MeleeWeapon.Instance.SetCanHit(false);
    }

    public void SetIsAttacking(bool choice)
    {
        isAttacking = choice;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool CanJump()
    {
        return canJump;
    }

    public bool CanRun()
    {
        return canRun;
    }

    public bool CanMove()
    {
        return canMove;
    }
}
