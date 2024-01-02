using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestSystem
{
    public void QuestStart();
    public void QuestStatus();
    public void QuestEnd();
    public bool GetQuestStatusCompleted();
    public bool GetQuestStatusActivated();

}
