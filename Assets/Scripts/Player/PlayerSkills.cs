using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using V10;

public class PlayerSkills : MonoBehaviour
{


    public static PlayerSkills Instance { get; private set; }


    public class SkillData
    {
        public bool unlocked;
    }

    [SerializeField] private List<UltimateSkillSO> ultimateSkills;
    [SerializeField] private List<SkillSO> skills;

    private List<SkillData> serializedUltimateSkills;
    private List<SkillData> serializedSkills;

    private Dictionary<SkillSO, SkillData> skillUnlockData;
    private Dictionary<UltimateSkillSO, SkillData> ultimateSkillUnlockData;

    private int equippedSkillIndex = 0;

    private Animator animator;

    private bool canUseSkill = true;
    private bool canUseUltimate = true;
    private bool isUsingUltimate = false;
    private bool isUsingSkill = false;


    [SerializeField] private GameObject toxicBlastPrefab;
    [SerializeField] private GameObject illusionaryDecoyPrefab;

    [SerializeField] private float damageBonus = 10.0f;
    [SerializeField] private float damageDuration = 3.0f; 
    [SerializeField] private float damageInterval = 1.0f;

    private float ultimateCooldownTime = 5.0f;
    private float baseUltimateCooldownMultiplier = 1.0f;
    private float ultimateCooldownMultiplier = 1.0f;
    private float skillCooldownTime = 2.0f;
    private float baseSkillCooldownMultiplier = 1.0f;
    private float skillCooldownMultiplier = 1.0f;

    public float UltimateCooldownTime => ultimateCooldownTime;
    public float SkillCooldownTime => skillCooldownTime;

    public event Action OnCastSkill;
    public event Action OnCastUltimate;

    private enum SkillState
    {
        Inactive,
        Selecting,
        Following,
        Throwing
    }

    private SkillState skillState = SkillState.Inactive;
    private GameObject selectedObject = null;

    [SerializeField] private float forceMagnitude = 30.0f;


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
        GameInput.Instance.OnAttackMeleeAction += GameInput_OnAttackMeleeAction;
        PlayerUpgrades.Instance.OnUpgradeUnlocked += PlayerUpgrades_OnUpgradeUnlocked;

        InitializeSerializedSkillDataLists();
        InitializeSkillUnlockData();
    }

    private void InitializeSerializedSkillDataLists()
    {
        serializedSkills = new List<SkillData>(skills.Count);
        serializedUltimateSkills = new List<SkillData>(ultimateSkills.Count);

        for (int i = 0; i < skills.Count; i++)
        {
            serializedSkills.Add(new SkillData());
        }

        for (int i = 0; i < ultimateSkills.Count; i++)
        {
            serializedUltimateSkills.Add(new SkillData());
        }
    }

    private void InitializeSkillUnlockData()
    {
        skillUnlockData = new Dictionary<SkillSO, SkillData>();
        ultimateSkillUnlockData = new Dictionary<UltimateSkillSO, SkillData>();

        foreach (var skill in skills)
        {
            skillUnlockData.Add(skill, new SkillData());
        }

        foreach (var ultimateSkill in ultimateSkills)
        {
            ultimateSkillUnlockData.Add(ultimateSkill, new SkillData());
        }
    }

    private void GameInput_OnAttackMeleeAction(object sender, EventArgs e)
    {
        if (skillState == SkillState.Following)
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<ObjectControl>().ChangeMaterialToOriginal();

                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;

                    forceMagnitude = 30.0f;
                    rb.AddForce(Vector3.down * forceMagnitude, ForceMode.Impulse);

                    SkillSO equippedSkill = skills[equippedSkillIndex];

                    ObjectControl objectControlScript = selectedObject.GetComponent<ObjectControl>();
                    if (objectControlScript != null)
                    {
                        objectControlScript.Initialize(equippedSkill);
                    }
                }

                VisualEffect selectedObjectVisualEffect = selectedObject.GetComponentInChildren<VisualEffect>();
                selectedObjectVisualEffect.Stop();

                selectedObject = null;
                canUseSkill = false;
                skillState = SkillState.Inactive;

                StartCoroutine(SkillCooldown());
            }
        }
    }

    private void PlayerUpgrades_OnUpgradeUnlocked(object sender, PlayerUpgrades.OnUpgradeUnlockedEventArgs e)
    {
        switch(e.upgradeType)
        {
            case PlayerUpgrades.UpgradeType.Katana_UltimateSkill:
                UnlockUltimateSkill(0);
                break;
            case PlayerUpgrades.UpgradeType.Knife_UltimateSkill:
                UnlockUltimateSkill(1);
                break;
            case PlayerUpgrades.UpgradeType.Pistol_UltimateSkill:
                UnlockUltimateSkill(2);
                break;
            case PlayerUpgrades.UpgradeType.Rifle_UltimateSkill:
                UnlockUltimateSkill(3);
                break;
            case PlayerUpgrades.UpgradeType.IllusionaryDecoy_Skill:
                UnlockSkill(2);
                break;
            case PlayerUpgrades.UpgradeType.ObjectControl_Skill:
                UnlockSkill(0);
                break;
            case PlayerUpgrades.UpgradeType.ToxicBlast_Skill:
                UnlockSkill(1);
                break;
            case PlayerUpgrades.UpgradeType.Cooldown_1:
                DecreaseSkillCooldownMultiplierPermanent(0.05f);
                DecreaseUltimateCooldownMultiplierPermanent(0.05f);
                break;
            case PlayerUpgrades.UpgradeType.Cooldown_2:
                DecreaseSkillCooldownMultiplierPermanent(0.1f);
                DecreaseUltimateCooldownMultiplierPermanent(0.1f);
                break;
            case PlayerUpgrades.UpgradeType.Cooldown_3:
                DecreaseSkillCooldownMultiplierPermanent(0.15f);
                DecreaseUltimateCooldownMultiplierPermanent(0.15f);
                break;
            case PlayerUpgrades.UpgradeType.Cooldown_4:
                DecreaseSkillCooldownMultiplierPermanent(0.25f);
                DecreaseUltimateCooldownMultiplierPermanent(0.25f);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (skillState == SkillState.Following)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layerMask = ~(1 << LayerMask.NameToLayer("Selectable"));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                float distanceToHitPoint = Vector3.Distance(Camera.main.transform.position, hit.point);

                float distanceAboveSurface = 5.0f;

                Vector3 newPosition = ray.GetPoint(distanceToHitPoint - distanceAboveSurface);

                float smoothness = 10.0f;

                selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, newPosition, Time.deltaTime * smoothness);

                float rotationSmoothness = 15.0f;
                Quaternion targetRotation = Quaternion.identity; // zero rotation
                selectedObject.transform.rotation = Quaternion.Lerp(selectedObject.transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
            }
        }
    }

    private void GameInput_OnSkillAction(object sender, System.EventArgs e)
    {
        if (canUseSkill & !isUsingUltimate)
        {
            if (equippedSkillIndex >= 0 && equippedSkillIndex < skills.Count)
            {
                SkillSO equippedSkill = skills[equippedSkillIndex];

                if (equippedSkill == null || !skillUnlockData[equippedSkill].unlocked) return;

                isUsingSkill = true;

                skillCooldownTime = equippedSkill.cooldown * skillCooldownMultiplier;
                ExecuteSkill(equippedSkill);
            }
        }
    }

    private void GameInput_OnUltimateRangeAction(object sender, System.EventArgs e)
    {
        if (canUseUltimate && !isUsingUltimate && !isUsingSkill)
        {
            string currentWeapon = RangedWeaponsSelector.Instance.GetActiveWeaponName();

            if (currentWeapon == null)
            {
                return;
            }

            UltimateSkillSO ultimateSkill = ultimateSkills.Find(skill => skill.weapon.ToString() == currentWeapon);

            if (ultimateSkill != null && ultimateSkillUnlockData[ultimateSkill].unlocked)
            {
                isUsingUltimate = true;

                ultimateCooldownTime = ultimateSkill.cooldown * ultimateCooldownMultiplier;
                ExecuteUltimate(ultimateSkill);
                canUseUltimate = false;
                StartCoroutine(UltimateCooldown());
            }
        }
    }

    private void GameInput_OnUltimateMeleeAction(object sender, System.EventArgs e)
    {
        if (canUseUltimate && !isUsingUltimate && !isUsingSkill)
        {
            string currentWeapon = MeleeWeaponsSelector.Instance.GetActiveWeaponName();

            if (currentWeapon == null)
            {
                return;
            }

            UltimateSkillSO ultimateSkill = ultimateSkills.Find(skill => skill.weapon.ToString() == currentWeapon);

            if (ultimateSkill != null && ultimateSkillUnlockData[ultimateSkill].unlocked)
            {
                isUsingUltimate = true;

                ultimateCooldownTime = ultimateSkill.cooldown * ultimateCooldownMultiplier;
                ExecuteUltimate(ultimateSkill);
                canUseUltimate = false;
                StartCoroutine(UltimateCooldown());
            }

        }
    }

    private void ExecuteSkill(SkillSO skill)
    {
        switch (skill.name)
        {
            case "ObjectControl_Skill":
                Skill1();
                break;

            case "ToxicBlast_Skill":
                Skill2(skill);
                break;

            case "IllusionaryDecoy_Skill":
                Skill3(skill);
                break;

            default:
                break;
        }
    }

    private void Skill1()
    { 
        if (skillState == SkillState.Following)
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<ObjectControl>().ChangeMaterialToOriginal();

                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.useGravity = true;
                }

                VisualEffect selectedObjectVisualEffect = selectedObject.GetComponentInChildren<VisualEffect>();
                selectedObjectVisualEffect.Stop();

                selectedObject = null;
                canUseSkill = false;
                skillState = SkillState.Inactive;

                StartCoroutine(SkillCooldown());
            }
        }
        else
        {
            skillState = SkillState.Selecting;

            Debug.Log("Select");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit point: " + hit.point);
                Debug.Log("Hit object tag: " + hit.collider.tag);

                if (hit.collider.CompareTag("Selectable"))
                {
                    selectedObject = hit.collider.gameObject;

                    selectedObject.GetComponent<ObjectControl>().ChangeMaterialToSelected();

                    ParticleSystem selectedObjectParticleSystem = selectedObject.GetComponentInChildren<ParticleSystem>();
                    selectedObjectParticleSystem.Play();

                    VisualEffect selectedObjectVisualEffect = selectedObject.GetComponentInChildren<VisualEffect>();
                    selectedObjectVisualEffect.Play();

                    Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false;
                    }

                    skillState = SkillState.Following;

                    Debug.Log("Follow");
                }
            }
        }
    }

    private void Skill2(SkillSO skill)
    {
        GameObject toxicBlastObject = Instantiate(toxicBlastPrefab, transform.position, Quaternion.identity);
        ToxicBlast toxicBlast = toxicBlastObject.GetComponent<ToxicBlast>();

        Vector3 decoySize = Vector3.zero;
        Collider decoyCollider = toxicBlastObject.GetComponent<Collider>();
        if (decoyCollider != null)
        {
            decoySize = decoyCollider.bounds.size;
        }

        // Adjust the Y position to avoid spawning inside the ground.
        Vector3 adjustedPosition = toxicBlastObject.transform.position;
        adjustedPosition.y += decoySize.y * 0.5f; // Half of the decoy's height.
        toxicBlastObject.transform.position = adjustedPosition;

        toxicBlast.Initialize(skill);

        canUseSkill = false;
        StartCoroutine(SkillCooldown());
    }

    private void Skill3(SkillSO skill)
    {
        Vector3 spawnPosition = transform.position - transform.forward * 2;
        GameObject decoy = Instantiate(illusionaryDecoyPrefab, spawnPosition, Quaternion.identity);

        IllusionaryDecoy decoyScript = decoy.GetComponent<IllusionaryDecoy>();
        decoyScript.Initialize(skill);

        canUseSkill = false;
        StartCoroutine(SkillCooldown());
    }

    private void ExecuteUltimate(UltimateSkillSO ultimateSkill)
    {
        Debug.Log(ultimateSkill.weapon.ToString());

        switch (ultimateSkill.weapon)
        {
            case Weapons.Knife:
                UltimateKnife(ultimateSkill);
                break;

            case Weapons.Katana: 
                UltimateKatana(ultimateSkill); 
                break;

            case Weapons.Pistol:
                UltimatePistol(ultimateSkill);
                break;

            case Weapons.Rifle:
                UltimateRifle(ultimateSkill);
                break;

            default: 
                break;
        }
    }

    private void UltimateKnife(UltimateSkillSO ultimateSkill)
    {
        WeaponSelector.Instance.ChangeSystem(0);

        animator.SetTrigger("Shoot");

        float eviscerateRange = 5.0f; 
        LayerMask enemyLayer = LayerMask.GetMask("Enemy"); 

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, eviscerateRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy != null)
            {
                StartCoroutine(DealDamageOverTime(enemy.gameObject, ultimateSkill));
            }
        }
    }

    private void UltimateKatana(UltimateSkillSO ultimateSkill)
    {
        WeaponSelector.Instance.ChangeSystem(0);

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
                hit.collider.GetComponent<HumanSummonAttack>()?.TakeDamage(ultimateSkill.damage);
                hit.collider.GetComponent<Enemy>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                hit.collider.GetComponent<Tank>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                hit.collider.GetComponent<Bullseye>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                hit.collider.GetComponent<PupeteerMeleeAttack>()?.TakeDamage(ultimateSkill.damage);
                hit.collider.GetComponent<Minder>()?.TakeDamage(ultimateSkill.damage);
                hit.collider.GetComponent<Venous>()?.TakeDamage(ultimateSkill.damage);
            }
        }
    }

    private void UltimateRifle(UltimateSkillSO ultimateSkill)
    {
        WeaponSelector.Instance.ChangeSystem(1);

        float coneAngle = 45f;
        float detectionRadius = 20f;

        Vector3 forwardDirection = transform.forward;
        Vector3 coneOrigin = transform.position;

        Collider[] colliders = Physics.OverlapSphere(coneOrigin, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = col.transform.position - coneOrigin;
                float angleToEnemy = Vector3.Angle(forwardDirection, directionToEnemy);

                if (angleToEnemy <= coneAngle / 2)
                {
                    StartCoroutine(DealDamageOverTime(col.gameObject, ultimateSkill));
                }
            }
        }
    }

    private void UltimatePistol(UltimateSkillSO ultimateSkill)
    {
        WeaponSelector.Instance.ChangeSystem(1);

        float detectionRadius = 20f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                StartCoroutine(DealDamageOverTime(col.gameObject, ultimateSkill));
            }
        }
    }

    IEnumerator DealDamageOverTime(GameObject enemy, UltimateSkillSO ultimateSkill)
    {
        if (enemy != null)
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
                    enemy.GetComponent<HumanSummonAttack>()?.TakeDamage(ultimateSkill.damage);
                    enemy.GetComponent<Enemy>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                    enemy.GetComponent<Tank>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                    enemy.GetComponent<Bullseye>()?.TakeHit(ultimateSkill.damageType, ultimateSkill.damage);
                    enemy.GetComponent<PupeteerMeleeAttack>()?.TakeDamage(ultimateSkill.damage);
                    enemy.GetComponent<Minder>()?.TakeDamage(ultimateSkill.damage);
                    enemy.GetComponent<Venous>()?.TakeDamage(ultimateSkill.damage);
                    timeSinceLastDamage = 0f; 
                }

                yield return null;
            }
        }
    }

    IEnumerator UltimateCooldown()
    {
        OnCastUltimate?.Invoke();
        isUsingUltimate = false;
        isUsingSkill = false;
        yield return new WaitForSeconds(ultimateCooldownTime);
        canUseUltimate = true;
    }


    IEnumerator SkillCooldown()
    {
        OnCastSkill?.Invoke();
        isUsingUltimate = false;
        isUsingSkill = false;
        yield return new WaitForSeconds(skillCooldownTime);
        canUseSkill = true;
    }


    public bool IsUsingUltimate()
    {
        return isUsingUltimate;
    }

    public bool IsUsingSkill()
    {
        return isUsingSkill;
    }

    public void ChangeSkill(int skillIndex)
    {
        equippedSkillIndex = skillIndex;
    }

    public void UnlockSkill(int skillIndex)
    {
        SkillSO equippedSkill = skills[skillIndex];
        skillUnlockData[equippedSkill].unlocked = true;
        equippedSkillIndex = skillIndex;
    }

    private void UnlockUltimateSkill(int ultimateSkillIndex)
    {
        UltimateSkillSO equippedUltimateSkill = ultimateSkills[ultimateSkillIndex];
        ultimateSkillUnlockData[equippedUltimateSkill].unlocked = true;
    }

    public void DecreaseUltimateCooldownMultiplierTemporarily(float amount)
    {
        ultimateCooldownMultiplier -= amount;
    }

    public void DecreaseUltimateCooldownMultiplierPermanent(float amount)
    {
        baseUltimateCooldownMultiplier -= amount;
        ultimateCooldownMultiplier = baseUltimateCooldownMultiplier;
    }

    public void ResetUltimateCooldownMultiplier()
    {
        ultimateCooldownMultiplier = baseUltimateCooldownMultiplier;
    }

    public void DecreaseSkillCooldownMultiplierTemporarily(float amount)
    {
        skillCooldownMultiplier -= amount;
    }

    public void DecreaseSkillCooldownMultiplierPermanent(float amount)
    {
        baseSkillCooldownMultiplier -= amount;
        skillCooldownMultiplier = baseSkillCooldownMultiplier;
    }

    public void ResetSkillCooldownMultiplier()
    {
        skillCooldownMultiplier = baseSkillCooldownMultiplier;
    }


}
