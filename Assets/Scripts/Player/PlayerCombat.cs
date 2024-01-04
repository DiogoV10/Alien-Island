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

    public enum RangeWeapons
    {
        Pistol,
        Rifle,
    }


    [SerializeField] private List<ComboTransition> comboTransitions;


    [Header("Range Weapon Settings")]
    [SerializeField] private float shootInterval = 0.5f;
    private float shootTimer = 1f;


    private Animator animator;
    private ComboSO currentCombo;

    private ComboCondition comboCondition;

    private int comboCounter;
    private int clipNumber = 1;


    private float damageMultiplier = 1f;
    private float baseDamageMultiplier = 1f;


    private bool canShoot = true;
    private bool canMove = true;
    private bool canRun = true;
    private bool canJump = true;
    private bool canAttack = true;
    private bool nextAttack = true;
    private bool buttonPressed = false;
    private bool isAttacking = false;
    private bool isShooting = false;
    private bool isHoldingShootingButton = false;

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
        GameInput.Instance.OnAttackRangeStartAction += GameInput_OnAttackRangeStartAction;
        GameInput.Instance.OnAttackRangeFinishAction += GameInput_OnAttackRangeFinishAction;
        PlayerUpgrades.Instance.OnUpgradeUnlocked += PlayerUpgrades_OnUpgradeUnlocked;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        ChangeRigWeight.Instance.SetRigWeight(0f);

        ResetCombo();
    }

    private void PlayerUpgrades_OnUpgradeUnlocked(object sender, PlayerUpgrades.OnUpgradeUnlockedEventArgs e)
    {
        switch (e.upgradeType)
        {
            case PlayerUpgrades.UpgradeType.Damage_1:
                IncreaseDamageMultiplierPermanent(0.5f);
                break;
            case PlayerUpgrades.UpgradeType.Damage_2:
                IncreaseDamageMultiplierPermanent(1f);
                break;
            case PlayerUpgrades.UpgradeType.Damage_3:
                IncreaseDamageMultiplierPermanent(2.5f);
                break;
            default:
                break;
        }
    }

    private void GameInput_OnAttackRangeFinishAction(object sender, EventArgs e)
    {
        if (canAttack && canShoot && RangedWeaponsSelector.Instance.GetWeaponCount() > 0 && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            if (WeaponSelector.Instance.GetCurrentRangeWeapon() == RangeWeapons.Pistol.ToString())
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
            }
        }

        if (WeaponSelector.Instance.GetCurrentRangeWeapon() == RangeWeapons.Rifle.ToString())
        {
            isHoldingShootingButton = false;
            UntoggleLockOn();
            InterruptCombo();
            CanShoot();
        }
    }

    private void GameInput_OnAttackRangeStartAction(object sender, System.EventArgs e)
    {
        if (canAttack && canShoot && RangedWeaponsSelector.Instance.GetWeaponCount() > 0 && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            if (WeaponSelector.Instance.GetCurrentRangeWeapon() == RangeWeapons.Rifle.ToString())
            {
                isHoldingShootingButton = true;

                ChangeRigWeight.Instance.SetRigWeight(1f);

                WeaponSelector.Instance.ChangeSystem(1);

                PlayerMovement.Instance.ToggleFaceObject(true);

                buttonPressed = false;

                InterruptCombo();

                canShoot = false;
                canRun = false;
                isShooting = true;
            }
        }
    }

    private void GameInput_OnAttackMeleeHoldAction(object sender, System.EventArgs e)
    {
        if (canAttack && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            WeaponSelector.Instance.ChangeSystem(0);
            ChangeRigWeight.Instance.SetRigWeight(0f);

            comboCondition = ComboCondition.Hold;

            buttonPressed = true;
            canAttack = false;
        }
    }

    private void GameInput_OnAttackMeleeAction(object sender, System.EventArgs e)
    {
        if (canAttack && !PlayerSkills.Instance.IsUsingUltimate() && !PlayerSkills.Instance.IsUsingSkill())
        {
            WeaponSelector.Instance.ChangeSystem(0);
            ChangeRigWeight.Instance.SetRigWeight(0f);

            buttonPressed = true;
            canAttack = false;

            var weapon = MeleeWeaponsSelector.Instance.GetActiveWeaponSO();
            if (weapon != null)
            {
                AudioManager.Instance.PlaySoundAt(weapon.attackSounds, transform.position);
            }
        }
    }

    private void Update()
    {
        if (!gameManagerCanCombat) return;

        if (nextAttack && buttonPressed)
        {
            Attack();
        }

        if ((PlayerMovement.Instance.IsRunning() || PlayerMovement.Instance.IsWalking()) && canMove && !isShooting)
        {
            InterruptCombo();
        }

        if (canMove && !isAttacking && !isShooting)
        {
            InterruptCombo();
        }

        currentWeapon = WeaponSelector.Instance.GetCurrentMeleeWeapon();
        pendingWeapon = WeaponSelector.Instance.GetPendingMeleeWeapon();

        if (!CheckComboWeapon())
        {
            MeleeWeaponsSelector.Instance.ChangeWeaponRequest();
        }

        HandleHoldShooting();
    }

    private void HandleHoldShooting()
    {
        if (!isHoldingShootingButton) return;

        if (WeaponSelector.Instance.GetCurrentRangeWeapon() == RangeWeapons.Pistol.ToString() || WeaponSelector.Instance.GetPendingRangeWeapon() == RangeWeapons.Pistol.ToString())
        {
            isHoldingShootingButton = false;
            UntoggleLockOn();
            InterruptCombo();
            CanShoot();
            return;
        }

        if (isHoldingShootingButton)
        {
            PlayerMovement.Instance.ToggleFaceObject(true);

            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                animator.SetTrigger("ShootRifle");
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 1f;
        }
    }

    private void Attack()
    {
        CheckComboTransitions();
        CheckComboWeapon();

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

        if (!PlayerMovement.Instance.IsGrounded())
        {
            PlayerGravity.Instance.AddForceOnAirAttack(2f);
            //PlayerGravity.Instance.DisableCanUseGravity();
        }

        clipNumber++;
        nextAttack = false;

        PlayerMovement.Instance.RotatePlayerTowardsInput();
    }

    public bool CheckComboTransitions()
    {
        foreach (ComboTransition transition in comboTransitions)
        {
            if (transition.sourceCombo == currentCombo &&
                transition.sourceWeapon.ToString() == currentWeapon &&
                transition.targetWeapon.ToString() == pendingWeapon &&
                comboCounter - 1 == transition.attackIndex &&
                transition.transitionCondition == comboCondition)
            {
                currentCombo = transition.targetCombo;
                comboCounter = 0;
                return true;
            }
        }

        return false;
    }

    public bool CheckComboWeapon()
    {
        if (currentCombo != null && currentCombo.weapon.ToString() == currentWeapon)
        {
            return true;
        }

        MeleeWeaponsSelector.Instance.ChangeWeaponRequest();

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

        comboCondition = ComboCondition.Wait;
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
        RangedWeaponsSelector.Instance.ChangeWeaponRequest();
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
        comboCondition = ComboCondition.None;

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
            return currentCombo.combo[comboCounter - 1].damage * damageMultiplier;
        else
            return currentCombo.combo[comboCounter].damage * damageMultiplier;
    }

    public void IncreaseDamageMultiplierTemporarily(float amount)
    {
        damageMultiplier += amount;
    }

    public void IncreaseDamageMultiplierPermanent(float amount)
    {
        baseDamageMultiplier += amount;
        damageMultiplier = baseDamageMultiplier;
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = baseDamageMultiplier;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }


}
