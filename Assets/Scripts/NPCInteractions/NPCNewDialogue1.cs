using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Dialogue : MonoBehaviour
{
    //public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    //private string filePath;
    InputSystem inputSystem;
    private int index;
    [SerializeField] List<GameObject> npc = new List<GameObject>();
    int playerMask = 3;


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
        gameObject.GetComponent<NPCDialogue>().speechString = string.Empty;
        //StartDialogue();
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
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            if (npc.Contains(this.gameObject))
            {
                gameObject.GetComponent<NPCDialogue>().speechString += c;
            }
            
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            gameObject.GetComponent<NPCDialogue>().speechString = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void NextLineTrigger()
    {
        if (gameObject.GetComponent<NPCDialogue>().speechString == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            gameObject.GetComponent<NPCDialogue>().speechString = lines[index];
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
                }
            }
        }
    }
}


