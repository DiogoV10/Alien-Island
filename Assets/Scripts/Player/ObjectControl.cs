using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ObjectControl : MonoBehaviour
{


    [SerializeField] private GameObject groundcrackPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material selectedMaterial;


    private Material originalMaterial;


    private bool hasHitGround = true;
    private SkillSO equippedSkill;


    public void Initialize(SkillSO skill)
    {
        equippedSkill = skill;
        hasHitGround = false;
    }

    private void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitGround && groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            hasHitGround = true;

            Instantiate(groundcrackPrefab, collision.GetContact(0).point, Quaternion.identity);

            // Deal damage to enemies within the specified range
            Collider[] colliders = Physics.OverlapSphere(transform.position, equippedSkill.range);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    float distanceToCenter = Vector3.Distance(collider.transform.position, transform.position);
                    float damage = CalculateDamage(distanceToCenter, equippedSkill);

                    collider.GetComponent<HumanSummonAttack>()?.TakeDamage(damage);
                    collider.GetComponent<Enemy>()?.TakeHit(BaseEnemy.DamageType.Big, damage);
                    collider.GetComponent<Tank>()?.TakeHit(BaseEnemy.DamageType.Big, damage);
                    collider.GetComponent<Bullseye>()?.TakeHit(BaseEnemy.DamageType.Big, damage);
                    collider.GetComponent<PupeteerMeleeAttack>()?.TakeDamage(damage);
                    collider.GetComponent<Minder>()?.TakeDamage(damage);
                    collider.GetComponent<Venous>()?.TakeDamage(damage);

                    Debug.Log("Dealing damage: " + damage);
                }
            }

            VisualEffect visualEffect = GetComponentInChildren<VisualEffect>();
            if (visualEffect != null)
            {
                visualEffect.Stop();
            }
        }
    }

    private float CalculateDamage(float distanceToCenter, SkillSO skill)
    {
        float normalizedDistance = Mathf.Clamp01(distanceToCenter / skill.range);
        float bonusDamage = 20.0f;

        return skill.damage + (bonusDamage * (1.0f - normalizedDistance));
    }

    public void ChangeMaterialToSelected()
    {
        GetComponent<Renderer>().material = selectedMaterial;
    }

    public void ChangeMaterialToOriginal()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }


}
