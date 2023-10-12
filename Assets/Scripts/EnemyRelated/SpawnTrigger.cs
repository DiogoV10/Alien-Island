using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

    public bool CanSpawnEnemy;

    private void Start()
    {
        CanSpawnEnemy = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanSpawnEnemy = true;
            CanSpawnEnemy = false;
            gameObject.SetActive(false);
            
        }
    }
}
