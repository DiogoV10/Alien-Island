using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniInventory : MonoBehaviour
{
    PlayerHealthManager playerHealthManager;
    InputSystem inputSystem;

    public int healthKitCount = 0;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Interactions.ConsumeHealthKit.performed += i => ConsumeHealthKit();
        }

        inputSystem.Enable();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = GetComponent<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerHealthManager.playerHealth);
    }

    private void ConsumeHealthKit()
    {
        if(healthKitCount > 0)
        {
            playerHealthManager.playerHealth += 20;
            healthKitCount -= 1;
            if (playerHealthManager.playerHealth > 100) playerHealthManager.playerHealth = 100;
        }
    }
}
