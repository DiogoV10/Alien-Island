using UnityEngine;

public class RangedWeaponsSwap : MonoBehaviour
{
    [Header("RangedWeapons")]
    [SerializeField] private GameObject[] rangedWeapons;

    [Header("Swap Key")]
    [SerializeField] private KeyCode swapKey;

    private int lastSelectedWeaponIndex;
    private bool isSelectorActive = true;

    void Start()
    {
        lastSelectedWeaponIndex = 0;
        SetActiveWeapon(lastSelectedWeaponIndex);
    }

    void Update()
    {
        if (isSelectorActive && Input.GetKeyDown(swapKey))
        {
            lastSelectedWeaponIndex = (lastSelectedWeaponIndex + 1) % rangedWeapons.Length;
            SetActiveWeapon(lastSelectedWeaponIndex);
            Debug.Log("Ranged Weapon Selected: " + GetActiveWeaponName());
        }
    }

    public void SetActive(bool isActive)
    {
        isSelectorActive = isActive;
        gameObject.SetActive(isActive);
    }

    private void SetActiveWeapon(int weaponIndex)
    {
        foreach (GameObject weapon in rangedWeapons)
        {
            weapon.SetActive(false);
        }

        rangedWeapons[weaponIndex].SetActive(true);
    }

    public bool IsActive()
    {
        return isSelectorActive;
    }

    public string GetActiveWeaponName()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < rangedWeapons.Length)
        {
            GameObject activeWeapon = rangedWeapons[lastSelectedWeaponIndex];
            if (activeWeapon != null)
            {
                return activeWeapon.name;
            }
        }
        return "Nenhuma arma ativa";
    }

    public int GetWeaponCount()
    {
        return rangedWeapons.Length;
    }

    public void SwitchToWeapon(int weaponIndex)
    {
        lastSelectedWeaponIndex = Mathf.Clamp(weaponIndex, 0, rangedWeapons.Length - 1);
        SetActiveWeapon(lastSelectedWeaponIndex);
    }
}
