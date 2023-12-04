using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitInteraction : MonoBehaviour
{
     [SerializeField] PlayerMiniInventory healthKit;

    int playerMask = 3;
    float healthKitLifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthKitLifeTime -= Time.deltaTime;
        if (healthKitLifeTime < 0.1) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == playerMask)
        {
            if(healthKit.healthKitCount < 5)
            {
                healthKit.healthKitCount += 1;
                Destroy(this.gameObject);
            }
        }
    }

    public void SetHealthKit(GameObject _player)
    {
        healthKit = _player.GetComponent<PlayerMiniInventory>();
    }
}
