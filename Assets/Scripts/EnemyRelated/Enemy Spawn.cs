using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform spawnPos;

    public GameObject enemyToSpawn;
    public SpawnTrigger spawnTrigger;

    
    void Update()
    {
        if (spawnTrigger.CanSpawnEnemy == true)
        {
            Instantiate(enemyToSpawn);
        }
    }
}
