using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{


    public float health;
    public float maxHealth;
    public float[] position;
    public bool[] unlockedUpgrades;

    public PlayerData(Player player)
    {
        health = PlayerHealthManager.Instance.GetHealth();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        unlockedUpgrades = PlayerUpgrades.Instance.GetUpgradeTypeArray();
    }


}
