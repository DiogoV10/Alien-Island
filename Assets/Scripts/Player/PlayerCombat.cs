using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V10;

public class PlayerCombat : MonoBehaviour
{


    public static PlayerCombat Instance { get; private set; }


    [SerializeField] private List<ComboSO> availableCombos;
    //[SerializeField] Weapon weapon;


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

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        currentCombo = availableCombos[0];
    }

    private void GameInput_OnAttackMeleeHoldAction(object sender, System.EventArgs e)
    {
        if (canAttack && canChangeCombo)
        {
            buttonPressed = true;
            canAttack = false;
            nextCombo = availableCombos[1];
            ChangeCombo();
        }
    }

    private void GameInput_OnAttackMeleeAction(object sender, System.EventArgs e)
    {
        if (canAttack && currentCombo.comboType == ComboType.Normal)
        {
            buttonPressed = true;
            canAttack = false;
            canChangeCombo = false;
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

        Debug.Log(comboCounter);
        Debug.Log(currentCombo.comboType.ToString());
    }

    void Attack()
    {
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
        //weapon.damage = combo[comboCounter].damage;

        nextAttack = false;
        buttonPressed = false;
        clipNumber++;
    }

    public void NextAttack()
    { 
        nextAttack = true;
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

        //Debug.Log("CanAttack");
        canAttack = true;
        buttonPressed = false;
        nextAttack = false;
    }

    void EndCombo()
    {
        Debug.Log("End");

        canAttack = true;
        buttonPressed = false;
        nextAttack = true;
        comboCounter = 0;
        isExitingCombo = false;
        canChangeCombo = false;

        //Debug.Log("Combo Reset: " + comboCounter);

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
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            isAttacking = false;
        else
            isAttacking = true;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
