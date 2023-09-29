using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponsSelector : MonoBehaviour
{
    [Header("Melee Weapons")]
    [SerializeField] private GameObject[] meleeWeapons;

    [Header("Swap Key")]
    [SerializeField] private KeyCode swapKey;

    [HideInInspector] public int lastSelectedWeaponIndex;
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
            lastSelectedWeaponIndex = (lastSelectedWeaponIndex + 1) % meleeWeapons.Length;
            SetActiveWeapon(lastSelectedWeaponIndex);
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
}