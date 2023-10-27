using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V10;

public class PlayerSkills : MonoBehaviour
{


    public static PlayerSkills Instance { get; private set; }


    private Animator animator;

    private string currentSkill;

    private bool canUseSkill = true;
    private bool canUseUltimate = true;
    private bool isUsingSkill = false;


    public float damageBonus = 10.0f; // Damage per second.
    public float damageDuration = 3.0f;  // Duration of the damage effect.
    public float damageInterval = 1.0f;

    public float ultimateCooldownTime = 5.0f;   // Cooldown time in seconds.
    public float skillCooldownTime = 2.0f;   // Cooldown time in seconds.
    private float lastSkillTime;        // Time when the skill was last used.


    private enum SkillState
    {
        Inactive,
        Selecting,
        Following,
        Throwing
    }

    private SkillState skillState = SkillState.Inactive;
    public GameObject selectedObject = null;
    private Vector3 throwDirection;

    public float throwForce = 10.0f;


    private void Awake()
    {

        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameInput.Instance.OnUltimateMeleeAction += GameInput_OnUltimateMeleeAction;
        GameInput.Instance.OnUltimateRangeAction += GameInput_OnUltimateRangeAction;
        GameInput.Instance.OnSkillAction += GameInput_OnSkillAction;
    }

    private void Update()
    {
        if (skillState == SkillState.Following)
        {
            selectedObject.transform.position = transform.position + new Vector3(4.0f, 3.0f, 0.0f);
        }
    }

    private void GameInput_OnSkillAction(object sender, System.EventArgs e)
    {
        if (canUseSkill)
        {
            Skill();
        }
    }

    private void GameInput_OnUltimateRangeAction(object sender, System.EventArgs e)
    {
        if (canUseUltimate && canUseSkill)
        {
            PlayerCombat.Instance.CannotAttack();
            isUsingSkill = true;
            UltimateRange();
            canUseUltimate = false;
            lastSkillTime = Time.time;
            StartCoroutine(UltimateCooldown());
        }
    }

    private void GameInput_OnUltimateMeleeAction(object sender, System.EventArgs e)
    {
        if (canUseUltimate && canUseSkill)
        {
            PlayerCombat.Instance.CannotAttack();
            isUsingSkill = true;
            UltimateMelee();
            canUseUltimate = false;
            lastSkillTime = Time.time;
            StartCoroutine(UltimateCooldown());
        }
    }

    private void Skill()
    {
        if (skillState == SkillState.Following)
        {
            if (selectedObject != null)
            {
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        // Calculate the throwDirection based on the raycast hit point.
                        throwDirection = (hit.point - selectedObject.transform.position).normalized;

                        // Apply the force using the calculated throwDirection.
                        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                    }
                }
                selectedObject = null;
                canUseSkill = false;
                lastSkillTime = Time.time;
                StartCoroutine(SkillCooldown());
                skillState = SkillState.Inactive;
            }
        }
        else
        {
            skillState = SkillState.Selecting;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.CompareTag("Selectable"))
                {
                    selectedObject = hit.collider.gameObject;
                    skillState = SkillState.Following;
                }
            }
        }
    }

    private void UltimateMelee()
    {
        WeaponSelector.Instance.ChangeSystem(0);

        PlayerCombat.Instance.CannotAttack();

        animator.SetTrigger("Shoot");

        float meleeRange = 30.0f;
        float meleeWidth = 10.0f;
        float meleeHeight = 5.0f;

        Vector3 meleeAreaSize = new Vector3(meleeWidth, meleeHeight, meleeRange);

        Vector3 meleeAreaPosition = transform.position + transform.forward * (meleeRange * 0.5f);

        Quaternion meleeAreaRotation = transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(meleeAreaPosition, meleeAreaSize * 0.5f, transform.forward, meleeAreaRotation, meleeRange);


        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {

                hit.collider.GetComponent<Health>().TakeDamage(100);
            }
        }
    }

    private void UltimateRange()
    {
        WeaponSelector.Instance.ChangeSystem(1);

        PlayerCombat.Instance.CannotAttack();

        float detectionRadius = 20f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                StartCoroutine(DealDamageOverTime(col.gameObject));
            }
        }
    }

    IEnumerator DealDamageOverTime(GameObject enemy)
    {
        if (enemy != null && !enemy.Equals(null))
        {
            Health enemyHealth = enemy.GetComponent<Health>();

            if (enemyHealth != null)
            {
                float startTime = Time.time;
                float timeSinceLastDamage = 0f;

                while (Time.time - startTime < damageDuration)
                {
                    if (enemy == null || enemy.Equals(null))
                    {
                        yield break; 
                    }

                    timeSinceLastDamage += Time.deltaTime;

                    if (timeSinceLastDamage >= damageInterval)
                    {
                        animator.SetTrigger("Shoot");
                        enemyHealth.TakeDamage(damageBonus + RangedWeapon.Instance.GunDamage());
                        timeSinceLastDamage = 0f; 
                    }

                    yield return null;
                }
            }
            else
            {
                Debug.LogError("The enemy does not have a Health component.");
            }
        }
        else
        {
            Debug.LogError("The enemy object is null.");
        }
    }

    IEnumerator UltimateCooldown()
    {
        yield return new WaitForSeconds(ultimateCooldownTime);
        canUseUltimate = true;
        PlayerCombat.Instance.CanAttack();
        isUsingSkill = false;
    }


    IEnumerator SkillCooldown()
    {
        yield return new WaitForSeconds(skillCooldownTime);
        canUseSkill = true;
        PlayerCombat.Instance.CanAttack();
        isUsingSkill = false;
    }


    public bool IsUsingSkill()
    {
        return isUsingSkill;
    }


}
