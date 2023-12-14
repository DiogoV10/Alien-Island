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
        public Weapons sourceWeapon; // Source weapon name.
        public Weapons targetWeapon; // Target weapon name.
    }

    public enum ComboCondition
    {
        None,
        Wait,
        Hold,
    }

    public enum Weapons
    {
        Knife,
        Katana,
    }


    [SerializeField] private List<ComboTransition> comboTransitions;

    [SerializeField] private Animator animator;


    private ComboSO currentCombo;


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
    private bool isShooting = false;

    private bool[] animationActive = new bool[2];

    private string currentWeapon;
    private string pendingWeapon;

    private AnimatorOverrideController animatorOverrideController;

    private bool gameManagerCanCombat = true;

    

    private void GameManagerOnGameStateChange(GameState state)
    {
        gameManagerCanCombat = state == GameState.InGame;
    }

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();

        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void Start()
    {
        GameInput.Instance.OnAttackMeleeAction += GameInput_OnAttackMeleeAction;
        GameInput.Instance.OnAttackMeleeHoldAction += GameInput_OnAttackMeleeHoldAction;
        GameInput.Instance.OnAttackRangeAction += GameInput_OnAttackRangeAction;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        ChangeRigWeight.Instance.SetRigWeight(0f);

        ResetCombo();
    }

    private void GameInput_OnAttackRangeAction(object sender, System.EventArgs e)
    {
        if (canAttack && canShoot && RangedWeaponsSelector.Instance.GetWeaponCount() > 0 && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            ChangeRigWeight.Instance.SetRigWeight(1f);

            animator.SetTrigger("Shoot");
            WeaponSelector.Instance.ChangeSystem(1);

            PlayerMovement.Instance.ToggleFaceObject(true);

            buttonPressed = false;

            InterruptCombo();

            canShoot = false;
            canRun = false;
            isShooting = true;

            RangedWeaponsSelector.Instance.ChangeWeaponRequest();
        }
    }

    private void GameInput_OnAttackMeleeHoldAction(object sender, System.EventArgs e)
    {
        if (canAttack && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
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
        if (canAttack && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            WeaponSelector.Instance.ChangeSystem(0);
            ChangeRigWeight.Instance.SetRigWeight(0f);

            CheckComboTransitions(ComboCondition.None);

            buttonPressed = true;
            canAttack = false;
        }
    }

    private void Update()
    {
        if (!gameManagerCanCombat) return;

        if (nextAttack && buttonPressed)
        {
            Attack();
        }

        if ((PlayerMovement.Instance.IsRunning() || PlayerMovement.Instance.IsWalking()) && canMove)
        {
            InterruptCombo();
        }

        currentWeapon = WeaponSelector.Instance.GetCurrentWeaponInHand();
        pendingWeapon = WeaponSelector.Instance.GetPendingWeaponInHand();
    }

    void Attack()
    {
        buttonPressed = false;
        isShooting = false;

        if (clipNumber > 2)
            clipNumber = 1;

        if (currentWeapon == null || currentCombo == null)
        {
            return;
        }

        if (comboCounter + 1 > currentCombo.combo.Count)
        {
            return;
        }

        canMove = false;
        canRun = false;
        canJump = false;
        isAttacking = true;

        animationActive[clipNumber - 1] = !animationActive[clipNumber - 1];

        animatorOverrideController["Attack" + clipNumber] = currentCombo.combo[comboCounter].animationClip;

        animator.SetTrigger("Attack" + clipNumber);

        if (PlayerMovement.Instance.IsFalling())
        {
            PlayerMovement.Instance.AddForceOnAirAttack(2f);
        }

        clipNumber++;
        nextAttack = false;

        PlayerMovement.Instance.RotatePlayerTowardsInput();

        MeleeWeaponsSelector.Instance.ChangeWeaponRequest();
    }

    public bool CheckComboTransitions(ComboCondition condition)
    {
        foreach (ComboTransition transition in comboTransitions)
        {
            if (transition.sourceCombo == currentCombo &&
                transition.sourceWeapon.ToString() == currentWeapon &&
                transition.targetWeapon.ToString() == pendingWeapon &&
                comboCounter - 1 == transition.attackIndex &&
                transition.transitionCondition == condition)
            {
                currentCombo = transition.targetCombo;
                comboCounter = 0;
                return true;
            }
        }

        //if (!IsAttacking() && !canMove)
        //{
        //    ResetCombo();
        //}

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
        foreach (ComboTransition transition in comboTransitions)
        {
            if (transition.sourceWeapon.ToString() == currentWeapon)
            {
                currentCombo = transition.targetCombo;
                comboCounter = 0;
                return;
            }
        }
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
        isShooting = false;

        animationActive[0] = true;
        animationActive[1] = true;

        ResetCombo();
        CheckComboTransitions(ComboCondition.None);

        MeleeWeaponsSelector.Instance.ChangeWeaponRequest();
        RangedWeaponsSelector.Instance.ChangeWeaponRequest();
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
    
    public bool IsShooting()
    {
        return isShooting;
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

    public Enemy.DamageType GetDamageType()
    {
        if (comboCounter + 1 > currentCombo.combo.Count)
            return currentCombo.combo[comboCounter-1].damageType;
        else
            return currentCombo.combo[comboCounter].damageType;
    }

    public float GetDamage()
    {
        if (comboCounter + 1 > currentCombo.combo.Count)
            return currentCombo.combo[comboCounter - 1].damage;
        else
            return currentCombo.combo[comboCounter].damage;
    }


}
