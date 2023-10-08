using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V10;

public class PlayerCombat : MonoBehaviour
{


    [SerializeField] private List<AttackSO> combo;
    [SerializeField] private Animator animator;
    //[SerializeField] Weapon weapon;


    private int comboCounter;
    private int clipNumber = 1;
    private bool canAttack = true;
    private bool nextAttack = true;
    private bool buttonPressed = false;
    private bool ending;

    private AnimatorOverrideController animatorOverrideController;


    private void Start()
    {
        GameInput.Instance.OnAttackMeleeAction += GameInput_OnAttackMeleeAction;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    private void GameInput_OnAttackMeleeAction(object sender, System.EventArgs e)
    {
        if (canAttack)
        {
            buttonPressed = true;
            canAttack = false;
        }
    }

    private void Update()
    {
        if (nextAttack && buttonPressed)
        {
            Attack();
        }

        ExitAttack();
    }

    void Attack()
    {
        if (ending)
        {
            CancelInvoke("EndCombo");
            ending = false;
        }
        

        if (clipNumber > 2)
            clipNumber = 1;

        animatorOverrideController["Attack" + clipNumber] = combo[comboCounter].animationClip;

        animator.CrossFade("Attack" + clipNumber, 0.2f);
        //weapon.damage = combo[comboCounter].damage;

        nextAttack = false;
        buttonPressed = false;
        clipNumber++;
    }

    public void NextAttack()
    {
        comboCounter++;

        if (comboCounter + 1 > combo.Count)
        {
            comboCounter = 0;
        }

        Debug.Log(comboCounter);
        Debug.Log("Can attack: " + comboCounter);
        nextAttack = true;
    }

    public void ChangeCombo()
    {

    }

    public void ExitAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !ending)
        {
            ending = true;
            Debug.Log("ExitAttack " + comboCounter);
            Invoke("EndCombo", combo[comboCounter].nextAttackDelay);
        }
    }

    public void CanAttack()
    {
        canAttack = true;
    }

    void EndCombo()
    {
        canAttack = true;
        buttonPressed = false;
        nextAttack = true;
        comboCounter = 0;
        Debug.Log("Combo Reset: " + comboCounter);
    }
}
