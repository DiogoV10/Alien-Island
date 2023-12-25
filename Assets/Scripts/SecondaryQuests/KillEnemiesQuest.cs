using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesQuest : MonoBehaviour, IQuestSystem
{

    [SerializeField] List<BaseEnemy> enemiesToKill;

    int killCount = 0;

    [SerializeField] bool questActivated = false;
    [SerializeField] bool questCompleted = false;

    public void QuestEnd()
    {
        questCompleted = true;
        //questActivated = false;
    }

    public void QuestStart()
    {
        questActivated = true;
    }

    public void QuestStatus()
    {
        foreach (BaseEnemy obj in enemiesToKill) 
        {
            if (obj.GetHealth() <= 0) 
            {
                enemiesToKill.Remove(obj);
                killCount += 1;
            } 
        }

        if (enemiesToKill.Count == 0) QuestEnd();
    }

    // Update is called once per frame
    void Update()
    {
        if (!questActivated) return;
        QuestStatus();
    }

    public bool GetQuestStatusCompleted()
    {
        return questCompleted;
    }

    public bool GetQuestStatusActivated()
    {
        return questActivated;
    }
}
