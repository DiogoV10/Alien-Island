using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject gunObject;
    [SerializeField] private RangedWeaponSystem weaponData;


    private void Awake()
    {
        weaponData.currentAmmo = weaponData.magSize;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
