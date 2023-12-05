using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitInteraction : MonoBehaviour
{
     [SerializeField] PlayerMiniInventory healthKit;

    int playerMask = 3;
    [SerializeField] float healthKitLifeTime = 8f, levitationTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MedKitLevitationAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        levitationTime -= Time.deltaTime;
        healthKitLifeTime -= Time.deltaTime;
        if (healthKitLifeTime < 0.1) Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            if (healthKit.healthKitCount < 1)
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

    IEnumerator MedKitLevitationAnimation()
    {
        float distanceStep = 0.1f;
        while (levitationTime > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + distanceStep, transform.position.z), 0.2f * Time.deltaTime);
            if(levitationTime < 0.05f)
            {
                distanceStep = -distanceStep;
                levitationTime = 2f;
            }
            yield return null;
        }
    }
}
