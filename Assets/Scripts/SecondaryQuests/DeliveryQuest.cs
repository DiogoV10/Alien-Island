using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQuest : MonoBehaviour, IQuestSystem
{

    [SerializeField] GameObject target;
    [SerializeField] GameObject objectivePoint;

    [SerializeField] bool questActivated = false;
    [SerializeField] bool questCompleted = false;
    [SerializeField] bool gotObject = false;
    [SerializeField] bool objDelivered = false;

    public bool GetQuestStatusActivated()
    {
        return questActivated;
    }

    public bool GetQuestStatusCompleted()
    {
        return questCompleted;
    }

    public void QuestEnd()
    {
        questActivated = false;
        objDelivered = true;
        questCompleted = true;
        SkillPoints.Instance.IncreaseSkillPoints();//Gained 1 skillPoint
    }

    public void QuestStart()
    {
        questActivated = true;
        gotObject = true;
        GameObject go = Instantiate(objectivePoint, target.transform.position, objectivePoint.transform.rotation);
        go.GetComponent<ObjectiveEffect>().GetDeliveryQuest(this);
    }

    public void QuestStatus()
    {
        throw new System.NotImplementedException();
    }

    public bool GettingObjectStatus()
    {
        return gotObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!questActivated) return;
        //else if (!questCompleted) QuestStatus();
    }
}
