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

    // Start is called before the first frame update
    void Start()
    {
        npcText.GetComponent<ChatBubble>();
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
            if (gameObject == npc[i])
            {
                ReadFromFile(i);
                switch (i)
                {
                    case 4:
                        KillEnemiesQuest killEnemiesQuest = gameObject.GetComponent<KillEnemiesQuest>();
                        if (killEnemiesQuest.GetQuestStatusCompleted() == false)
                        {
                            if (!hasInteracted && indialogue)
                            {
                                if (killEnemiesQuest.GetQuestStatusActivated() == false) killEnemiesQuest.QuestStart();
                                hasInteracted = true;
                            }
                            StartDialogue(0);
                        }

                        else if (killEnemiesQuest.GetQuestStatusCompleted() == true)
                        {
                            StartDialogue(linesQuestCheckCount + 1);
                        }
                        
                        break;

                    case 5:
                        GetObjectQuest getObjectQuest = gameObject.GetComponent<GetObjectQuest>();

                        

                        if (!getObjectQuest.GetQuestStatusCompleted() && !getObjectQuest.GettingObjectStatus() )
                        {
                            if (!hasInteracted && indialogue)
                            {
                                if (getObjectQuest.GetQuestStatusActivated() == false) getObjectQuest.QuestStart();
                                hasInteracted = true;
                            }
                            StartDialogue(0);
                        }

                        else if (getObjectQuest.GetQuestStatusCompleted())
                        {
                            StartDialogue(linesQuestCheckCount + 1);
                        }

                        else if (getObjectQuest.GettingObjectStatus())
                        {
                            getObjectQuest.QuestEnd();
                            StartDialogue(linesQuestCheckCount + 1);
                        } 
                        break;

                    default:
                        StartDialogue(0);
                        break;
                }
            }
        }
    }

    public void SetNPCText(Transform _ChatBubleText)
    {
        npcText = _ChatBubleText;
    }
}


