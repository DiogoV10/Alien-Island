using UnityEngine;

public class RangedWeaponsSelector : MonoBehaviour
{


    public static RangedWeaponsSelector Instance { get; private set; }


    [Header("RangedWeapons")]
    [SerializeField] private RangedWeaponSO[] rangedWeaponSOs;

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

        if (weaponIndex >= 0 && weaponIndex < rangedWeaponSOs.Length)
        {
            // Instantiate the selected weapon GameObject from the Scriptable Object.
            RangedWeaponSO activeWeaponSO = rangedWeaponSOs[weaponIndex];
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
        return rangedWeaponSOs.Length;
    }

    public string GetActiveWeaponName()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < rangedWeaponSOs.Length)
        {
            RangedWeaponSO activeWeaponSO = rangedWeaponSOs[lastSelectedWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO.name;
            }
        }
        return "Nenhuma arma ativa";
    }

    public string GetPendingWeaponName()
    {
        if (pendingWeaponIndex >= 0 && pendingWeaponIndex < rangedWeaponSOs.Length)
        {
            RangedWeaponSO pendingWeaponSO = rangedWeaponSOs[pendingWeaponIndex];
            if (pendingWeaponSO != null)
            {
                return pendingWeaponSO.name;
            }
        }
        return "No pending weapon selected";
    }

    public void SwitchToWeapon(int weaponIndex)
    {
        lastSelectedWeaponIndex = Mathf.Clamp(weaponIndex, 0, rangedWeaponSOs.Length - 1);
        SetActiveWeapon(lastSelectedWeaponIndex);
    }

    public void RequestWeaponChange(int weaponIndex)
    {
        pendingWeaponIndex = Mathf.Clamp(weaponIndex, 0, rangedWeaponSOs.Length - 1);
    }

    public void ChangeWeaponRequest()
    {
        SwitchToWeapon(pendingWeaponIndex);
    }
}
