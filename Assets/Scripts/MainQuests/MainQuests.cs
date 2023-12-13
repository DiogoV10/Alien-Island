using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainQuests : MonoBehaviour, IQuestSystem
{
    public static MainQuests Instance;

    string[] mainQuests;
    string currentQuest;
    string status;

    int newQuest = 0;

    bool endGame = false;


    void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameState state)
    {
        endGame = state == GameState.NpcDialogue;
    }
    public void QuestEnd()
    {
        Debug.Log("End Quest");
        newQuest += 1;
        QuestStatus();
    }

    public void QuestStart()
    {
        currentQuest = mainQuests[newQuest];
        QuestStatus();
    }

    public void QuestStatus()
    {
        if (currentQuest == mainQuests[newQuest])
            status = "Active";
        else
        {
            status = "Completed";
            //TODO: if not game end
            if(!endGame) QuestStart();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string ReadFromFilePath = Application.streamingAssetsPath + "/Recall_Chat/" + "MainQuests.txt";
        mainQuests = File.ReadAllLines(ReadFromFilePath);
        currentQuest = mainQuests[0];
        QuestStatus();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current quest:" + currentQuest);
        //Debug.Log("quest status:" + status);
    }
}
