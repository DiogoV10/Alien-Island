using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private MeleeWeaponSystem weapon;


    private void Awake()
    {
        weapon.canAttack = true;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
