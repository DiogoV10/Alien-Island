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


    private bool adrenalineRushActive;
    private bool extraterrestrialAgility;

    private float adrenalineDuration = 10.0f;
    private float agilityDuration = 10.0f;

    private int meleeEnergyCharges = 0;
    private int maxMeleeEnergyCharges = 5;

    private int rangedEnergyCharges = 0;
    private int maxRangedEnergyCharges = 5;


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
            float healthThreshold = 0.25f;

            if (PlayerHealthManager.Instance.GetHealth() < PlayerHealthManager.Instance.GetMaxHealth() * healthThreshold && !adrenalineRushActive)
            {
                ActivateAdrenalineRush(adrenalineDuration);
            }
        }

        if (passiveSkills[PassiveSkills.ExtraterrestrialAgility].unlocked)
        {
            float healthThreshold = 0.25f;

            if (PlayerHealthManager.Instance.GetHealth() < PlayerHealthManager.Instance.GetMaxHealth() * healthThreshold && !extraterrestrialAgility)
            {
                ActivateExtraterrestrialAgility(agilityDuration);
            }
        }

        if (passiveSkills[PassiveSkills.EnergyChanneling].unlocked)
        {
            if (IsMeleeAttack())
            {
                if (rangedEnergyCharges > 0 && meleeEnergyCharges == 0)
                {
                    PlayerCombat.Instance.IncreaseDamageMultiplierTemporarily(rangedEnergyCharges / 2);
                }

                if (meleeEnergyCharges == 1 && rangedEnergyCharges > 0)
                {
                    if (adrenalineRushActive)
                    {
                        PlayerCombat.Instance.ReduceDamageMultiplier(rangedEnergyCharges);
                    }
                    else
                    {
                        PlayerCombat.Instance.ResetDamageMultiplier();
                    }

                    meleeEnergyCharges = 0;
                    rangedEnergyCharges = 0;
                }

                if (meleeEnergyCharges < maxMeleeEnergyCharges)
                {
                    meleeEnergyCharges++;
                }
            }
            else
            {
                if (meleeEnergyCharges > 0 && rangedEnergyCharges == 0)
                {
                    PlayerCombat.Instance.IncreaseDamageMultiplierTemporarily(meleeEnergyCharges / 2);
                }

                if (rangedEnergyCharges == 1 && meleeEnergyCharges > 0)
                {
                    if (adrenalineRushActive)
                    {
                        PlayerCombat.Instance.ReduceDamageMultiplier(meleeEnergyCharges);
                    }
                    else
                    {
                        PlayerCombat.Instance.ResetDamageMultiplier();
                    }
                    meleeEnergyCharges = 0;
                    rangedEnergyCharges = 0;
                }

                if (rangedEnergyCharges < maxRangedEnergyCharges)
                {
                    rangedEnergyCharges++;
                }
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

    private void ActivateAdrenalineRush(float adrenalineD)
    {
        adrenalineRushActive = true;

        float adrenalineMultiplier = 1.5f;
        float adrenalineCooldownMultiplier = 0.1f;

        PlayerCombat.Instance.IncreaseDamageMultiplierTemporarily(adrenalineMultiplier);
        PlayerSkills.Instance.DecreaseSkillCooldownMultiplierTemporarily(adrenalineCooldownMultiplier);
        PlayerSkills.Instance.DecreaseUltimateCooldownMultiplierTemporarily(adrenalineCooldownMultiplier);

        StartCoroutine(DeactivateAdrenalineRush(adrenalineD));
    }

    private void ActivateExtraterrestrialAgility(float adrenalineD)
    {
        extraterrestrialAgility = true;

        float healthReduction = 1.5f;

        PlayerCombat.Instance.IncreaseHealthReduction(healthReduction);

        StartCoroutine(DeactivateExtraterrestrialAgility(adrenalineD));
    }

    private IEnumerator DeactivateAdrenalineRush(float duration)
    {
        yield return new WaitForSeconds(duration);

        adrenalineRushActive = false;

        PlayerCombat.Instance.ResetDamageMultiplier();
        PlayerSkills.Instance.ResetSkillCooldownMultiplier();
        PlayerSkills.Instance.ResetUltimateCooldownMultiplier();
    }

    private IEnumerator DeactivateExtraterrestrialAgility(float duration)
    {
        yield return new WaitForSeconds(duration);

        extraterrestrialAgility = false;

        PlayerCombat.Instance.ResetHealthReduction();
    }

    private void InitializePassiveSkills()
    {
        passiveSkills = new Dictionary<PassiveSkills, PassiveSkill>();

        foreach (PassiveSkills passive in Enum.GetValues(typeof(PassiveSkills)))
        {
            passiveSkills.Add(passive, new PassiveSkill());
        }

        //passiveSkills[PassiveSkills.LifeSteal].unlocked = true;
        //passiveSkills[PassiveSkills.AdrenalineRush].unlocked = true;
        passiveSkills[PassiveSkills.EnergyChanneling].unlocked = true;
    }


}
