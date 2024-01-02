using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoints : MonoBehaviour
{

    public static SkillPoints Instance;

    int totalSkillPoints = 0;
    int killedEnemyPoints = 0;

    private void Awake()
    {
        Instance = this;
    }

    public int GetSkillPointsCount()
    {
        return totalSkillPoints;
    }

    public void IncreaseSkillPoints()
    {
        totalSkillPoints += 1;
    }

    public void DecreaseSkillPoints(int numPoints)
    {
        totalSkillPoints -= numPoints;
    }

    public int GetKilledEnemyPoints()
    {
        return killedEnemyPoints;
    }

    public void IncreaseEnemyKilledPoints(int _killedEnemyPoints)
    {
        if(GetKilledEnemyPoints() >= 1000)
        {
            IncreaseSkillPoints();
            killedEnemyPoints = 0;
            return;
        }
        killedEnemyPoints += _killedEnemyPoints;
        Debug.Log("KillPoints: " + killedEnemyPoints);
    }
}
