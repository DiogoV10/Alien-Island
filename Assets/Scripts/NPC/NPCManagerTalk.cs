using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManagerTalk : MonoBehaviour
{
    string one = "Please help us, the one who they call MINDER\nhas taken over the island " +
        "he went SOUTHWEST\n but be carefull his friends are all over the place and they\n are dangerous!!!";
    string two = "You See this fire it restores you're Life if\nyou interact with it, but it takes a little\ntime so you have to be patient.";
    string three = "There are enemies on the SOUTH too,\nI used to go to that forest but now it is\ninfested with those cretaures from the sky!";
    string four = "You´'re alive how is that possible?\nFor that I can tell you you're strong, so if you\ncan help us we have more probles on the SOUTH.";
    string five = "Before you go SOUTH you might\nwanna go NORTHEAST from here,\ncause i think i saw some creatures up there";

    List<string> speechList = new List<string>();
    [SerializeField] List<NPCDialogue> npc = new List<NPCDialogue>();
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        speechList.Add(one);
        speechList.Add(two);
        speechList.Add(three);
        speechList.Add(four);
        speechList.Add(five);
    }

    // Update is called once per frame
    void Update()
    {
        if (count != npc.Count) AttributeString();
    }

    void AttributeString()
    {
        if(npc != null)
        {
            for(int i = 0; i < npc.Count; i++)
            {
                npc[i].speechString = speechList[i];
                count = i;
            }
        }
    }
}
