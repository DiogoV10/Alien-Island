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
    [SerializeField] bool questActivated = false;
    [SerializeField] bool questCompleted = false;


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
        //Debug.Log("End Quest");
        questCompleted = true;
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
        if (mainQuests[newQuest] == "EndGame") return;//ToDo: EndGame

        else if (currentQuest == mainQuests[newQuest])
        {
            questActivated = true;
            questCompleted = false;
        }

        else
        {
            //TODO: if not game end
            if (!endGame) QuestStart();
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

    public bool GetQuestStatusCompleted()
    {
        return questCompleted;
    }

    public bool GetQuestStatusActivated()
    {
        return questActivated;
    }
}
