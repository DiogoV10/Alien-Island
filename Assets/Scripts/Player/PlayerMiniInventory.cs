using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniInventory : MonoBehaviour
{
    PlayerHealthManager playerHealthManager;
    InputSystem inputSystem;
    [SerializeField] GameObject medKit;
    [SerializeField] GameObject medKitPlace;

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
        Debug.Log(playerHealthManager.GetHealth());
    }

    private void ConsumeHealthKit()
    {
        if(healthKitCount > 0)
        {
            if (playerHealthManager.GetHealth() < playerHealthManager.GetMaxHealth())
            {
                playerHealthManager.IncrementCurrentHealth(15f);
                healthKitCount -= 1;
                if (playerHealthManager.GetHealth() > playerHealthManager.GetMaxHealth()) playerHealthManager.IncrementCurrentHealth(0f);
            }
            
            
        }
    }
}
