using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{


    public static WeaponSelector Instance { get; private set; }


    [Header("Systems Keys")]
    [SerializeField] private KeyCode meleeSystemKey;
    [SerializeField] private KeyCode rangedSystemKey;

    [Header("Misc")]
    [SerializeField] private float switchTime;

    public MeleeWeaponsSelector meleeWeaponsSelector;
    public RangedWeaponsSwap rangedWeaponsSelector;

    private float timeSinceLastWeaponSwitch;

    private int meleeWeaponIndex = 0;
    private int rangedWeaponIndex = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SelectSystem(0); 
        timeSinceLastWeaponSwitch = 0f;
    }

    private void Update()
    {
        timeSinceLastWeaponSwitch += Time.deltaTime;

        //Debug.Log("Yes");

        //if (Input.GetKeyDown(meleeSystemKey) && timeSinceLastWeaponSwitch >= switchTime)
        //{
        //    SelectSystem(0); 
        //}
        //else if (Input.GetKeyDown(rangedSystemKey) && timeSinceLastWeaponSwitch >= switchTime)
        //{
        //    SelectSystem(1); 
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1) && meleeWeaponsSelector.IsActive())
        {
            meleeWeaponIndex = (meleeWeaponIndex + 1) % meleeWeaponsSelector.GetWeaponCount();
            meleeWeaponsSelector.SwitchToWeapon(meleeWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && rangedWeaponsSelector.IsActive())
        {
            rangedWeaponIndex = (rangedWeaponIndex + 1) % rangedWeaponsSelector.GetWeaponCount();
            rangedWeaponsSelector.SwitchToWeapon(rangedWeaponIndex);
        }
    }

    private void SelectSystem(int systemIndex)
    {
        meleeWeaponsSelector.SetActive(systemIndex == 0);
        rangedWeaponsSelector.SetActive(systemIndex == 1);

        timeSinceLastWeaponSwitch = 0f;

        OnSystemSelected(systemIndex);
    }

    private void OnSystemSelected(int systemIndex)
    {
        
    }

    public void ChangeSystem(int systemIndex)
    {
        SelectSystem(systemIndex);
    }
}
