using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupWeapon : MonoBehaviour
{


    [SerializeField] private MeleeWeaponSO meleeWeapon;
    [SerializeField] private RangedWeaponSO rangedWeapon;
    [SerializeField] Transform buttonT;
    [SerializeField] private Camera mainCam;


    private Transform buttonInstance;
    private InputSystem inputSystem;


    private int playerMask = 3;
    private bool insideZone = false;


    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Interactions.Interact.performed += i => PickUpWeapon();
        }

        inputSystem.Enable();
    }

    private void Update()
    {
        if (buttonInstance != null)
        {
            buttonInstance.rotation = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0);
        }
    }

    private void PickUpWeapon()
    {
        if (!insideZone) return;

        PickupObjectAndAddWeapon();

        Destroy(gameObject);
    }

    private void PickupObjectAndAddWeapon()
    {
        if (meleeWeapon != null)
        {
            MeleeWeaponsSelector.Instance.AddWeapon(meleeWeapon);
        }
        else if (rangedWeapon != null)
        {
            RangedWeaponsSelector.Instance.AddWeapon(rangedWeapon);
        }
    }

    private Transform InstantiateInteractButton(Transform parent, Vector3 localPosition)
    {
        buttonInstance = Instantiate(buttonT, parent);
        buttonInstance.localPosition = localPosition;

        return buttonInstance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            insideZone = true;
            InstantiateInteractButton(transform, new Vector3(0, 2f, 0));
            Debug.Log("button");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            insideZone = false;
            Destroy(buttonInstance.gameObject);
        }
    }

    private void OnDisable()
    {
        if (inputSystem != null)
        {
            inputSystem.Disable();
            inputSystem.Dispose();
        }
    }

    private void OnDestroy()
    {
        OnDisable();
    }


}
