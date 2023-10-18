using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using V10;

public class PlayerCombat : MonoBehaviour
{


    public static PlayerCombat Instance { get; private set; }


    [SerializeField] private List<ComboSO> availableCombos;
    //[SerializeField] Weapon weapon;

    [SerializeField] public float playerHealth;

    private Animator animator;


    private ComboSO currentCombo;
    private ComboSO nextCombo;


    private int comboCounter;
    private int clipNumber = 1;

    private bool canChangeCombo = false;
    private bool canShoot = true;
    private bool isMelee = false;
    
    
    private bool canMove = true;
    private bool canRun = true;
    private bool canJump = true;
    private bool canAttack = true;
    private bool nextAttack = true;
    private bool buttonPressed = false;
    private bool isAttacking = false;


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
            isMelee = false;

            InterruptCombo();

            canShoot = false;
            canRun = false;
        }
    }

    private void GameInput_OnAttackMeleeHoldAction(object sender, System.EventArgs e)
    {
        if (canAttack && canChangeCombo)
        {
            WeaponSelector.Instance.ChangeSystem(0);

            buttonPressed = true;
            canAttack = false;
            nextCombo = availableCombos[1];
            ChangeCombo();
        }
    }

    private void GameInput_OnAttackMeleeAction(object sender, System.EventArgs e)
    {
        if (canAttack)
        {
            WeaponSelector.Instance.ChangeSystem(0);

            buttonPressed = true;
            //canChangeCombo = false;
            //isMelee = true;
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
    }

    void Attack()
    {
        animator.ResetTrigger("Attack" + (clipNumber - 1));

        if (clipNumber > 2)
            clipNumber = 1;

        animatorOverrideController["Attack" + clipNumber] = currentCombo.combo[comboCounter].animationClip;


        animator.SetTrigger("Attack" + clipNumber);
        //animator.Play("Attack" + clipNumber, 0, 0);
        //weapon.damage = combo[comboCounter].damage;

        if (PlayerMovement.Instance.IsFalling())
        {
            PlayerMovement.Instance.AddForceOnAirAttack(10f);
        }

        nextAttack = false;
        buttonPressed = false;
        clipNumber++;

        PlayerMovement.Instance.RotatePlayerTowardsInput();
    }

    public void NextAttack()
    { 
        nextAttack = true;
        canJump = true;
    }

    void ChangeCombo()
    {
        nextCombo = availableCombos[2];
        comboCounter = 0;
        currentCombo = nextCombo;
        Debug.Log(canMove);
    }

    void ResetCombo ()
    {
        nextCombo = availableCombos[0];
        comboCounter = 0;
        currentCombo = nextCombo;
    }

    public void IncrementCombo()
    {
        if (comboCounter + 2 > currentCombo.combo.Count)
        {
            //canChangeCombo = true;
            comboCounter = 0;
        }
        else
        {
            comboCounter++;
        }
    }

    public void CanMoveAndRun()
    {
        canMove = true;
        canRun = true;
        canAttack = true;
    }

    public void CanAttack()
    {
        canAttack = true;
        //buttonPressed = false;
        //nextAttack = false;
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
        isMelee = false;
        canAttack = true;
        buttonPressed = false;
        nextAttack = true;
        canChangeCombo = false;
        canJump = true;
        canMove = true;
        canRun = true;
        isAttacking = false;

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

    public bool IsMelee()
    {
        return isMelee;
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
