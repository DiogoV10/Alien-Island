using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{

    [SerializeField] GetObjectQuest quest;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuest(GetObjectQuest _quest)
    {
        quest = _quest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            quest.GettingObject();
            Destroy(this.gameObject);
        }
    }
}
