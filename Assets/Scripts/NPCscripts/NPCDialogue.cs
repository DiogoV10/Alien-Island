using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class NPCDialogue : MonoBehaviour
{

    string[] lines;

    int index;
    int playerMask = 3;
    int linesQuestCheckCount = 0;

    [SerializeField] bool hasInteracted = false, indialogue = false, dialogueStarted = false;
    Transform npcText;
    InputSystem inputSystem;

    [SerializeField] List<GameObject> npc = new List<GameObject>();

    void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }

    private void GameManagerOnGameStateChange(GameState state)
    {
        indialogue = state == GameState.NpcDialogue;
    }

    private void OnEnable()
    {
        Dialogue();

        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Interactions.NextLine.performed += i => NextLineTrigger();
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!indialogue)
        {
            dialogueStarted = false;
            linesQuestCheckCount = 0;
            return;
        }
        else if(!dialogueStarted) Dialogue();
        
    }

    void ReadFromFile(int npc)
    {
        string ReadFromFilePath = Application.streamingAssetsPath + "/Recall_Chat/" + "NPC" + (npc + 1) + ".txt";
        lines = File.ReadAllLines(ReadFromFilePath);
        foreach(string line in lines)
        {
            if (line == "Quest Completed Speech") break;
            linesQuestCheckCount += 1;
            if (linesQuestCheckCount > lines.Length) linesQuestCheckCount = lines.Length;
        }
    }
    void StartDialogue(int _index)
    {
        index = _index;
        gameObject.GetComponent<NPCInteraction>().speechString = lines[index];
        if(indialogue) dialogueStarted = true;
    }


    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            if(index != linesQuestCheckCount) npcText.gameObject.GetComponent<ChatBubble>().text = lines[index];
            if (index == lines.Length - 1 || index == linesQuestCheckCount - 1)
            {
                npcText.gameObject.GetComponent<ChatBubble>().pressButtonText = "Press T to Exit";
            }
        }
    }

    void NextLineTrigger()
    {
        if (npcText.gameObject.GetComponent<ChatBubble>().text == lines[index] && index != linesQuestCheckCount)
        {
            NextLine();
        }
        else if(index != linesQuestCheckCount)
        {
            npcText.gameObject.GetComponent<ChatBubble>().text = lines[index];
        }
    }

    private void Dialogue()
    {
        for (int i = 0; i < npc.Count; i++)
        {
            Debug.Log("i: " + i);
            if (gameObject == npc[i])
            {
                ReadFromFile(i);
                switch (i)
                {
                    case 4:
                        KillEnemiesQuest killEnemiesQuest = gameObject.GetComponent<KillEnemiesQuest>();
                        if (!killEnemiesQuest.enabled) killEnemiesQuest.enabled = true;
                        QuestsLogicCode(killEnemiesQuest);
                        
                        break;

                    case 5:
                        GetObjectQuest getObjectQuest = gameObject.GetComponent<GetObjectQuest>();
                        if (!getObjectQuest.enabled) getObjectQuest.enabled = true;
                        QuestsLogicCode(getObjectQuest);

                        break;

                    case 6:
                        DeliveryQuest deliveryQuest = gameObject.GetComponent<DeliveryQuest>();
                        if (!deliveryQuest.enabled) deliveryQuest.enabled = true;
                        QuestsLogicCode(deliveryQuest);

                        break;

                    default:
                        StartDialogue(0);
                        break;
                }
            }
        }
    }

    private void QuestsLogicCode(IQuestSystem quest)
    {
        if (!quest.GetQuestStatusCompleted())
        {
            if (!hasInteracted && indialogue)
            {
                if (quest.GetQuestStatusActivated() == false) quest.QuestStart();
                hasInteracted = true;
            }
            StartDialogue(0);
        }

        else if (quest.GetQuestStatusCompleted())
        {
            StartDialogue(linesQuestCheckCount + 1);
        }
    }

    public void SetNPCText(Transform _chatBubleText)
    {
        npcText = _chatBubleText;
    }

    


}


