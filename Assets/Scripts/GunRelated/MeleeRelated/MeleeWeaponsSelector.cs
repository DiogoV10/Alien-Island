using UnityEngine;

public class MeleeWeaponsSelector : MonoBehaviour
{
    [Header("Melee Weapons")]
    [SerializeField] private GameObject[] meleeWeapons;

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
        if (isSelectorActive && Input.GetKeyDown(swapKey) && !PlayerSkills.Instance.IsUsingSkill())
        {
            lastSelectedWeaponIndex = (lastSelectedWeaponIndex + 1) % meleeWeapons.Length;
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
        foreach (GameObject weapon in meleeWeapons)
        {
            weapon.SetActive(false);
        }

        meleeWeapons[weaponIndex].SetActive(true);
    }

    public bool IsActive()
    {
        return isSelectorActive;
    }

    public int GetWeaponCount()
    {
        return meleeWeapons.Length;
    }

    public string GetActiveWeaponName()
    {
        if (lastSelectedWeaponIndex >= 0 && lastSelectedWeaponIndex < meleeWeapons.Length)
        {
            GameObject activeWeapon = meleeWeapons[lastSelectedWeaponIndex];
            if (activeWeapon != null)
            {
                return activeWeapon.name;
            }
        }
        return "Nenhuma arma ativa";
    }

    public void SwitchToWeapon(int weaponIndex)
    {
        lastSelectedWeaponIndex = Mathf.Clamp(weaponIndex, 0, meleeWeapons.Length - 1);
        SetActiveWeapon(lastSelectedWeaponIndex);
    }
}
