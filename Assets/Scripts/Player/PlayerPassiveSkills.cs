using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveSkills : MonoBehaviour
{


    public static PlayerPassiveSkills Instance { get; private set; }


    public enum PassiveSkills
    {
        LifeSteal,
        AdrenalineRush,
        ExtraterrestrialAgility,
        EnergyChanneling,
    }

    public class PassiveSkill
    {
        public bool unlocked;
    }

    private Dictionary<PassiveSkills, PassiveSkill> passiveSkills;


    private int meleeEnergyCharges;
    private int maxMeleeEnergyCharges;

    private int rangedEnergyCharges;
    private int maxRangedEnergyCharges;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializePassiveSkills();

        MeleeWeapon.OnHitEnemy += MeleeWeapon_OnHitEnemy;
        RangedWeapon.OnHitEnemy += RangedWeapon_OnHitEnemy;
    }

    private void RangedWeapon_OnHitEnemy(object sender, EventArgs e)
    {
        OnHitEnemy();
    }

    private void MeleeWeapon_OnHitEnemy(object sender, EventArgs e)
    {
        OnHitEnemy();
    }

    private void OnDestroy()
    {
        MeleeWeapon.OnHitEnemy -= MeleeWeapon_OnHitEnemy;
        RangedWeapon.OnHitEnemy -= RangedWeapon_OnHitEnemy;
    }

    private void OnHitEnemy()
    {
        if (passiveSkills[PassiveSkills.LifeSteal].unlocked)
        {
            PlayerHealthManager.Instance.IncrementCurrentHealth(0.2f);
        }

        if (passiveSkills[PassiveSkills.AdrenalineRush].unlocked)
        {
            float healthThreshold = 0.99f;

            if (PlayerHealthManager.Instance.GetHealth() < PlayerHealthManager.Instance.GetMaxHealth() * healthThreshold)
            {
                ActivateAdrenalineRush();
            }
        }

        if (passiveSkills[PassiveSkills.EnergyChanneling].unlocked)
        {
            if (IsMeleeAttack())
            {
                
            }
            else
            {

            }
        }
    }

    private bool IsMeleeAttack()
    {
        if (WeaponSelector.Instance.GetCurrentWeaponInHand() == Weapons.Katana.ToString() || WeaponSelector.Instance.GetCurrentWeaponInHand() == Weapons.Knife.ToString())
            return true;
        else
            return false;
    }

    private void ActivateAdrenalineRush()
    {
        float adrenalineDuration = 10.0f;
        float adrenalineMultiplier = 1.5f;
        float adrenalineCooldownMultiplier = 0.1f;

        PlayerCombat.Instance.IncreaseDamageMultiplierTemporarily(adrenalineMultiplier);
        PlayerSkills.Instance.DecreaseSkillCooldownMultiplierTemporarily(adrenalineCooldownMultiplier);
        PlayerSkills.Instance.DecreaseUltimateCooldownMultiplierTemporarily(adrenalineCooldownMultiplier);

        StartCoroutine(DeactivateAdrenalineRush(adrenalineDuration));
    }

    private IEnumerator DeactivateAdrenalineRush(float duration)
    {
        yield return new WaitForSeconds(duration);

        PlayerCombat.Instance.ResetDamageMultiplier();
        PlayerSkills.Instance.ResetSkillCooldownMultiplier();
        PlayerSkills.Instance.ResetUltimateCooldownMultiplier();
    }

    private void InitializePassiveSkills()
    {
        passiveSkills = new Dictionary<PassiveSkills, PassiveSkill>();

        foreach (PassiveSkills passive in Enum.GetValues(typeof(PassiveSkills)))
        {
            passiveSkills.Add(passive, new PassiveSkill());
        }

        passiveSkills[PassiveSkills.LifeSteal].unlocked = true;
        passiveSkills[PassiveSkills.AdrenalineRush].unlocked = true;
    }


}
