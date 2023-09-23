using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{

    [Header("Weapons Systems")]
    [SerializeField] private Transform[] weaponsSystem;

    [Header("Systems Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Misc")]
    [SerializeField] private float switchTime;

    private int selectedSystem;
    private float timeSinceLastSystemSwitch;
    
    void Start()
    {
        SetSystems();
        timeSinceLastSystemSwitch = 0f;
    }

    
    void Update()
    {
        int previousSelectedSystem = selectedSystem;
    }

    public bool IsSwitching => timeSinceLastSystemSwitch != 0;

    public void SetSystems()
    {
        weaponsSystem = new Transform[transform.childCount];

        Debug.Log(weaponsSystem.Length);
    }
}
