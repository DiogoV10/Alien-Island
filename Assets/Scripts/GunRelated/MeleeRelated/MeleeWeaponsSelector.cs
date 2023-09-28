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


    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
