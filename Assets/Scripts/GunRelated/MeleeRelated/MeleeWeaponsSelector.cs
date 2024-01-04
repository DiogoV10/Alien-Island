using System;
using UnityEngine;

public class MeleeWeaponsSelector : MonoBehaviour
{


    public static MeleeWeaponsSelector Instance { get; private set; }


    [Header("Melee Weapons")]
    [SerializeField] private MeleeWeaponSO[] meleeWeaponSOs;

    private int lastSelectedWeaponIndex;
    private int pendingWeaponIndex = 0;

    private bool isSelectorActive = true;

    private GameObject activeWeaponObject;

    //Event
    public event Action<MeleeWeaponSO> OnChangeWeapon;


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

        if (weaponIndex >= 0 && weaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[weaponIndex];
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

    public MeleeWeaponSO GetActiveWeaponSO()
    {
        if (pendingWeaponIndex >= 0 && pendingWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[pendingWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO;
            }
        }
        return null;
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

    public float GetActiveWeaponDamage()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[lastSelectedWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO.damage + PlayerCombat.Instance.GetDamage();
            }
        }
        return 0;
    }

    public MeleeWeaponSO GetActiveWeaponSO()
    {
        if (pendingWeaponIndex >= 0 && pendingWeaponIndex < meleeWeaponSOs.Length)
        {
            MeleeWeaponSO activeWeaponSO = meleeWeaponSOs[pendingWeaponIndex];
            if (activeWeaponSO != null)
            {
                return activeWeaponSO;
            }
        }
        return null;
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

    public void AddWeapon(MeleeWeaponSO newWeapon)
    {
        if (!Array.Exists(meleeWeaponSOs, weapon => weapon == newWeapon))
        {
            Array.Resize(ref meleeWeaponSOs, meleeWeaponSOs.Length + 1);

            meleeWeaponSOs[meleeWeaponSOs.Length - 1] = newWeapon;

            if (activeWeaponObject == null)
            {
                WeaponSelector.Instance.ChangeMeleeWeapon();
                ChangeWeaponRequest();
                WeaponSelector.Instance.ChangeSystem(0);
            }
        }
    }


}
