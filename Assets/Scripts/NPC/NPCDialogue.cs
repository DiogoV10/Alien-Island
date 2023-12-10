using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class NPCDialogue : MonoBehaviour
{

    string[] lines;
    private int index;
    int playerMask = 3;
    Transform npcText;
    InputSystem inputSystem;

    [SerializeField] List<GameObject> npc = new List<GameObject>();


    private void OnEnable()
    {
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

    }

    public void StartDialogue(int npc)
    {
        string ReadFromFilePath = Application.streamingAssetsPath + "/Recall_Chat/" + "NPC" + (npc + 1) + ".txt";
        lines = File.ReadAllLines(ReadFromFilePath);
        index = 0;
        gameObject.GetComponent<NPCInteraction>().speechString = lines[index];
    }


    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            npcText.gameObject.GetComponent<ChatBubble>().text = lines[index];
            if(index == lines.Length - 1)
            {
                npcText.gameObject.GetComponent<ChatBubble>().pressButtonText = "Press T to Exit";
            }
        }
    }

    void NextLineTrigger()
    {
        if (npcText.gameObject.GetComponent<ChatBubble>().text == lines[index])
        {
            NextLine();
        }
        else
        {
            npcText.gameObject.GetComponent<ChatBubble>().text = lines[index];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {

            for (int i = 0; i < npc.Count; i++)
            {
                if (this.gameObject == npc[i])
                {
                    StartDialogue(i);
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


