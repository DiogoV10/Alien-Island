using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{

    [SerializeField] GetObjectQuest quest;

    public void SetQuest(GetObjectQuest _quest)
    {
        quest = _quest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            quest.GettingObject();
            quest.QuestEnd();
            Destroy(this.gameObject);
        }
    }
}
