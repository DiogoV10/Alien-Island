using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectQuest : MonoBehaviour, IQuestSystem
{

    [SerializeField] List<BaseEnemy> enemiesToKill;
    [SerializeField] GameObject questObject;

    [SerializeField] bool questActivated = false;
    [SerializeField] bool questCompleted = false;
    [SerializeField] bool gotObject = false, objInstantiated = false;

    public void QuestEnd()
    {
        questCompleted = true;
    }

    public void QuestStart()
    {
        questActivated = true;
    }

    public void QuestStatus()
    {
        Debug.Log("EnemieList: " + enemiesToKill.Count);
        Debug.Log("Quest Active: " + questActivated);

        if(enemiesToKill.Count < 3) 
        {
            foreach (BaseEnemy enemy in enemiesToKill)
            {
                if (enemy.GetHealth() <= 0)
                {
                    if (enemiesToKill.Count == 1 && !objInstantiated)
                    {
                        Debug.Log("Instantiating Object");
                        GameObject go = Instantiate(questObject, enemy.transform.position, enemy.transform.rotation);
                        go.GetComponent<Notebook>().SetQuest(this);
                        objInstantiated = true;
                    }

                    else
                    {
                        float spawnChance = Random.Range(0, 10);
                        if (spawnChance > 7)
                        {
                            Debug.Log("Instantiating Object");
                            GameObject go = Instantiate(questObject, enemy.transform.position, enemy.transform.rotation);
                            go.GetComponent<Notebook>().SetQuest(this);
                            objInstantiated = true;
                        }
                    }
                    enemiesToKill.Remove(enemy);
                }
            }
        }
        
    }

    public void GettingObject()
    {
        gotObject = true;
    }

    public bool GettingObjectStatus()
    {
        return gotObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!questActivated) return;
        else if(!questCompleted) QuestStatus();
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
