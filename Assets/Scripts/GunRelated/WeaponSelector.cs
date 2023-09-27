using JetBrains.Annotations;
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
        SelectSystem(selectedSystem);
        timeSinceLastSystemSwitch = 0f;
    }

    
    void Update()
    {
        int previousSelectedSystem = selectedSystem;

        for(int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSystemSwitch >= switchTime) selectedSystem = i;
        }

        if(previousSelectedSystem != selectedSystem) SelectSystem(selectedSystem);

        timeSinceLastSystemSwitch += Time.deltaTime;
    }

    public bool IsSwitching => timeSinceLastSystemSwitch != 0;

    public void SetSystems()
    {
        weaponsSystem = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weaponsSystem[i] = transform.GetChild(i);
        }

        if (keys == null) keys = new KeyCode[weaponsSystem.Length];
    }

    private void SelectSystem(int systemIndex)
    {   
        for(int i = 0; i < weaponsSystem.Length; i++)
        {
            weaponsSystem[i].gameObject.SetActive(i == systemIndex);
        }

        timeSinceLastSystemSwitch = 0f;

        OnSystemSelected();
    }

    private void OnSystemSelected()
    {

    }
}
