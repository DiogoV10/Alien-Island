// Ignore Spelling: Melee

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V10;

public class WeaponSelector : MonoBehaviour
{


    public static WeaponSelector Instance { get; private set; }


    [Header("Misc")]
    [SerializeField] private float switchTime;


    private float timeSinceLastWeaponSwitchMelee;
    private float timeSinceLastWeaponSwitchRange;

    private int meleeWeaponIndex = 0;
    private int rangedWeaponIndex = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnChangeMeleeWeapon += GameInput_OnChangeMeleeWeapon;
        GameInput.Instance.OnChangeRangeWeapon += GameInput_OnChangeRangeWeapon;

        SelectSystem(0);

        timeSinceLastWeaponSwitchMelee = 0f;
        timeSinceLastWeaponSwitchRange = 0f;
    }

    private void Update()
    {
        timeSinceLastWeaponSwitchMelee += Time.deltaTime;
        timeSinceLastWeaponSwitchRange += Time.deltaTime;
    }

    private void GameInput_OnChangeRangeWeapon(object sender, System.EventArgs e)
    {
        if (timeSinceLastWeaponSwitchRange >= switchTime && RangedWeaponsSelector.Instance.GetWeaponCount() > 0)
        {
            ChangeRangeWeapon();
                
            timeSinceLastWeaponSwitchRange = 0f;
        }
    }

    private void GameInput_OnChangeMeleeWeapon(object sender, System.EventArgs e)
    {
        if (timeSinceLastWeaponSwitchMelee >= switchTime && MeleeWeaponsSelector.Instance.GetWeaponCount() > 0)
        {
            ChangeMeleeWeapon();

            timeSinceLastWeaponSwitchMelee = 0f;
        }
    }

    public void ChangeMeleeWeapon()
    {
        if (MeleeWeaponsSelector.Instance.GetWeaponCount() <= 0) return;

        meleeWeaponIndex = (meleeWeaponIndex + 1) % MeleeWeaponsSelector.Instance.GetWeaponCount();
        MeleeWeaponsSelector.Instance.RequestWeaponChange(meleeWeaponIndex);

        if (!PlayerSkills.Instance.IsUsingUltimate() && !PlayerCombat.Instance.IsAttacking())
        {
            MeleeWeaponsSelector.Instance.ChangeWeaponRequest();
        }

        if (PlayerCombat.Instance.IsShooting())
        {
            MeleeWeaponsSelector.Instance.ChangeWeaponRequest();
        }
    }

    public void ChangeRangeWeapon()
    {
        if (RangedWeaponsSelector.Instance.GetWeaponCount() <= 0) return;

        rangedWeaponIndex = (rangedWeaponIndex + 1) % RangedWeaponsSelector.Instance.GetWeaponCount();
        RangedWeaponsSelector.Instance.RequestWeaponChange(rangedWeaponIndex);

        if (!PlayerSkills.Instance.IsUsingUltimate() && !PlayerCombat.Instance.IsShooting())
        {
            RangedWeaponsSelector.Instance.ChangeWeaponRequest();
        }

        if (PlayerCombat.Instance.IsAttacking())
        {
            RangedWeaponsSelector.Instance.ChangeWeaponRequest();
        }
    }

    private void SelectSystem(int systemIndex)
    {
        MeleeWeaponsSelector.Instance.SetActive(systemIndex == 0);
        RangedWeaponsSelector.Instance.SetActive(systemIndex == 1);
    }

    public void ChangeSystem(int systemIndex)
    {
        SelectSystem(systemIndex);
    }

    public string GetCurrentMeleeWeapon()
    {
        return MeleeWeaponsSelector.Instance.GetActiveWeaponName();
    }

    public string GetPendingMeleeWeapon()
    {
        return MeleeWeaponsSelector.Instance.GetPendingWeaponName();
    }

    public string GetCurrentRangeWeapon()
    {
        return RangedWeaponsSelector.Instance.GetActiveWeaponName();
    }

    public string GetCurrentWeaponInHand()
    {
        if (MeleeWeaponsSelector.Instance.IsActive())
        {
            return MeleeWeaponsSelector.Instance.GetActiveWeaponName();
        }
        else if (RangedWeaponsSelector.Instance.IsActive())
        {
            return RangedWeaponsSelector.Instance.GetActiveWeaponName();
        }

        return "No weapon selected";
    }

    public string GetPendingWeaponInHand()
    {
        if (MeleeWeaponsSelector.Instance.IsActive())
        {
            return MeleeWeaponsSelector.Instance.GetPendingWeaponName();
        }
        else if (RangedWeaponsSelector.Instance.IsActive())
        {
            return RangedWeaponsSelector.Instance.GetPendingWeaponName();
        }

        return "No pending weapon selected";
    }
}
