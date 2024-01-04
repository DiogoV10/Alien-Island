using System;
using UnityEngine;

public class RangedWeaponsSelector : MonoBehaviour
{


    public static RangedWeaponsSelector Instance { get; private set; }


    [Header("RangedWeapons")]
    [SerializeField] private RangedWeaponSO[] rangedWeaponSOs;

    private int lastSelectedWeaponIndex;
    private int pendingWeaponIndex = 0;

    private bool isSelectorActive = true;

    private GameObject activeWeaponObject;

    //Event
    public event Action<RangedWeaponSO> OnChangeWeapon;


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
            Destroy(activeWeaponObject);
        }

        if (weaponIndex >= 0 && weaponIndex < rangedWeaponSOs.Length)
        {
            RangedWeaponSO activeWeaponSO = rangedWeaponSOs[weaponIndex];
            if (activeWeaponSO != null && activeWeaponSO.weaponPrefab != null)
            {
                activeWeaponObject = Instantiate(activeWeaponSO.weaponPrefab, transform);
                OnChangeWeapon?.Invoke(activeWeaponSO);
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

    public float GetActiveWeaponDamage()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < rangedWeaponSOs.Length)
        {
            RangedWeaponSO activeWeaponSO = rangedWeaponSOs[lastSelectedWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO.damage * PlayerCombat.Instance.GetDamageMultiplier();
            }
        }
        return 0;
    }

    public RangedWeaponSO GetActiveWeaponSO()
    {
        if (pendingWeaponIndex >= 0 && pendingWeaponIndex < rangedWeaponSOs.Length)
        {
            RangedWeaponSO activeWeaponSO = rangedWeaponSOs[pendingWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO;
            }
        }
        return null;
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

    public void AddWeapon(RangedWeaponSO newWeapon)
    {
        if (!Array.Exists(rangedWeaponSOs, weapon => weapon == newWeapon))
        {
            Array.Resize(ref rangedWeaponSOs, rangedWeaponSOs.Length + 1);

            rangedWeaponSOs[rangedWeaponSOs.Length - 1] = newWeapon;

            if (activeWeaponObject == null)
            {
                WeaponSelector.Instance.ChangeRangeWeapon();
                ChangeWeaponRequest();
                WeaponSelector.Instance.ChangeSystem(1);
            }
        }
    }



}
