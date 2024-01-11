using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{


    public float health;
    public int upgradePoints;
    public float[] position;
    public bool[] unlockedUpgrades;

    public PlayerData(Player player)
    {
        health = PlayerHealthManager.Instance.GetHealth();
        upgradePoints = PlayerUpgrades.Instance.GetUpgradePoints();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        unlockedUpgrades = PlayerUpgrades.Instance.GetUpgradeTypeArray();
    }


}
