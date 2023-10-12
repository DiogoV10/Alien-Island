using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool canAttack = true;
    private bool nextAttack = true;
    private bool buttonPressed = false;
    private bool isExitingCombo;
    private bool endCombo = false;
    private bool isAttacking = false;
    private bool canChangeCombo = false;
    private bool canShoot = true;
    private bool isMelee = false;
    private bool canJump = true;
    private bool canRun = true;

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

        currentCombo = availableCombos[0];
    }

    private void GameInput_OnAttackRangeAction(object sender, System.EventArgs e)
    {
        if (canAttack && canShoot)
        {
            animator.SetTrigger("Shoot");
            WeaponSelector.Instance.ChangeSystem(1);

            PlayerMovement.Instance.ToggleFaceObject(true);

            buttonPressed = false;
            isAttacking = false;
            isMelee = false;

            ExitCombo();

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
        if (canAttack && currentCombo.comboType == ComboType.Normal && canShoot)
        {
            WeaponSelector.Instance.ChangeSystem(0);

            buttonPressed = true;
            canChangeCombo = false;
            isMelee = true;
            isAttacking = true;

            canAttack = false;
        }
    }

    private void Update()
    {
        HasAnimationEnded();

        if (nextAttack && buttonPressed)
        {
            Attack();
        }

        if ((PlayerMovement.Instance.IsRunning() || PlayerMovement.Instance.IsWalking()) && !isAttacking)
        {
            ExitCombo();
        }
    }

    void Attack()
    {
        canJump = false;
        canRun = true;

        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("Shoot") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            isAttacking = true;

        if (isExitingCombo)
        {
            //Debug.Log("CancelInvoke");
            CancelInvoke("EndCombo");
            isExitingCombo = false;
        }

        if (clipNumber > 2)
            clipNumber = 1;

        animatorOverrideController["Attack" + clipNumber] = currentCombo.combo[comboCounter].animationClip;


        animator.Play("Attack" + clipNumber, 0, 0);
        //animator.Play("Attack" + clipNumber, 0, 0);
        //weapon.damage = combo[comboCounter].damage;

        if (PlayerMovement.Instance.IsFalling())
        {
            PlayerMovement.Instance.AddForceOnAirAttack(2);
        }

        nextAttack = false;
        buttonPressed = false;
        clipNumber++;
    }

    public void NextAttack()
    { 
        nextAttack = true;
        isMelee = false;
        canJump = true;
    }

    void ChangeCombo()
    {
        comboCounter = 0;
        currentCombo = nextCombo;
    }

    public void ExitAttack()
    {
        if (!isExitingCombo)
        {
            //Debug.Log("ExitAttack " + comboCounter);

            if (canChangeCombo)
            {
                Debug.Log("Wait");
                Invoke("EndCombo", currentCombo.nextComboDelay);
            }else
                Invoke("EndCombo", currentCombo.combo[comboCounter].nextAttackDelay);

            endCombo = false;
            isExitingCombo = true;
        }
    }

    public void CanAttack()
    {
        if (comboCounter + 2 > currentCombo.combo.Count)
        {
            canChangeCombo = true;
            comboCounter = 0;
        }
        else
        {
            comboCounter++;
        }

        canAttack = true;
        buttonPressed = false;
        nextAttack = false;
    }

    public void CanShoot()
    {
        canShoot = true;
    }

    public void UntoggleLockOn()
    {
        PlayerMovement.Instance.ToggleFaceObject(false);
        canRun = true;
    }

    void EndCombo()
    {
        isMelee = false;
        canAttack = true;
        buttonPressed = false;
        nextAttack = true;
        comboCounter = 0;
        isExitingCombo = false;
        canChangeCombo = false;
        canJump = true;

        currentCombo = availableCombos[0];
    }

    void ExitCombo()
    {
        if (!endCombo)
        {
            CancelInvoke("EndCombo");
            EndCombo();
            endCombo = true;
        }
    }

    private void HasAnimationEnded()
    {
        if (isMelee)
            isAttacking = true;
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || animator.GetCurrentAnimatorStateInfo(1).IsTag("Shoot"))
            isAttacking = false;
    }

    public void CanHit()
    {
        MeleeWeapon.Instance.SetCanHit(true);
    }

    public void CannotHit()
    {
        MeleeWeapon.Instance.SetCanHit(false);
    }

    public bool IsAttacking()
    {
        return isAttacking;
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
}
