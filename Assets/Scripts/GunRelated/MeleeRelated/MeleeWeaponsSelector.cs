using UnityEngine;

public class MeleeWeaponsSelector : MonoBehaviour
{


    public static MeleeWeaponsSelector Instance { get; private set; }


    [Header("Melee Weapons")]
    [SerializeField] private MeleeWeaponSO[] meleeWeaponSOs;

    private int lastSelectedWeaponIndex;
    private int pendingWeaponIndex = 0; // Index of the weapon to switch to.

    private bool isSelectorActive = true;

    private GameObject activeWeaponObject;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lastSelectedWeaponIndex = 0;
        SetActiveWeapon(lastSelectedWeaponIndex);
    }

    private void Update()
    {
        //if (changeWeaponRequested)
        //{
        //    SwitchToWeapon(pendingWeaponIndex);
        //    PlayerCombat.Instance.CheckComboTransitions(PlayerCombat.ComboCondition.None);
        //    changeWeaponRequested = false; // Reset the flag after changing the weapon.
        //}
    }

    public void SetActive(bool isActive)
    {
        isSelectorActive = isActive;
        gameObject.SetActive(isActive);
    }

    private void SetActiveWeapon(int weaponIndex)
    {
        if (activeWeaponObject != null)
        {
            // Destroy the previously active weapon if one exists.
            Destroy(activeWeaponObject);
        }

        if (weaponIndex >= 0 && weaponIndex < meleeWeaponSOs.Length)
        {
            // Instantiate the selected weapon GameObject from the Scriptable Object.
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[weaponIndex];
            if (activeWeaponSO != null && activeWeaponSO.weaponPrefab != null)
            {
                activeWeaponObject = Instantiate(activeWeaponSO.weaponPrefab, transform);
            }
        }
    }

    public bool IsActive()
    {
        return isSelectorActive;
    }

    public int GetWeaponCount()
    {
        return meleeWeaponSOs.Length;
    }

    public string GetActiveWeaponName()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[lastSelectedWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO.name;
            }
        }
        return "Nenhuma arma ativa";
    }

    public string GetPendingWeaponName()
    {
        if (pendingWeaponIndex >= 0 && pendingWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO pendingWeaponSO = meleeWeaponSOs[pendingWeaponIndex];
            if (pendingWeaponSO != null)
            {
                return pendingWeaponSO.name;
            }
        }
        return "No pending weapon selected";
    }

    public int GetActiveWeaponDamage()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[lastSelectedWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO.damage;
            }
        }
        return 0;
    }

    public void SwitchToWeapon(int weaponIndex)
    {
        lastSelectedWeaponIndex = Mathf.Clamp(weaponIndex, 0, meleeWeaponSOs.Length - 1);
        SetActiveWeapon(lastSelectedWeaponIndex);
    }

    public void RequestWeaponChange(int weaponIndex)
    {
        pendingWeaponIndex = Mathf.Clamp(weaponIndex, 0, meleeWeaponSOs.Length - 1);
    }

    public void ChangeWeaponRequest()
    {
        SwitchToWeapon(pendingWeaponIndex);
    }
}
