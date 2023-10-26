using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pupeteer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] private float timeToNextAttack = 3f;

    [Header("InvokeHumansAttack")]
    [SerializeField] private float invokeHumanPuppetsTime = 20f, timeBetweenPuppets = 4f;
    [SerializeField] GameObject humanPuppet;
    List<GameObject> listOfHumanPuppets = new List<GameObject>();
    private bool invokeHumanPuppetsOn = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        ChooseAttack();
        InvokeHumanPuppets();
        //RemoveDeadEnemiesFromList();
    }

    void InvokeHumanPuppets()
    {
        if (invokeHumanPuppetsOn && listOfHumanPuppets.Count < 5)
        {
            if (invokeHumanPuppetsTime < 0f)
            {
                invokeHumanPuppetsOn = false;
                invokeHumanPuppetsTime = 20f;
                timeBetweenPuppets = 4f;
                return;
            }

            timeBetweenPuppets -= Time.deltaTime;
            invokeHumanPuppetsTime -= Time.deltaTime;
            
            if (timeBetweenPuppets <= 0f)
            {
                GameObject go = Instantiate(humanPuppet, new Vector3(10, 1, 20), Quaternion.identity);
                go.GetComponent<HumanSummonAttack>().SetPlayer(player);
                go.GetComponent<HumanSummonAttack>().SetCallback(RemoveFromEnemiesList);
                listOfHumanPuppets.Add(go);
                timeBetweenPuppets = 4f;
            }
        }
        
        else
        {
            invokeHumanPuppetsOn = false;
            invokeHumanPuppetsTime = 20f;
            timeBetweenPuppets = 4f;
        }
    }

    void RemoveFromEnemiesList(GameObject go)
    {
        Debug.Log("Tou sim: " + listOfHumanPuppets.Contains(go));
        listOfHumanPuppets.Remove(go);
        Debug.Log("Removed");
        Destroy(go);
        Debug.Log("destroyed");
    }

    void RemoveDeadEnemiesFromList()
    {
        foreach (GameObject humanPuppet in listOfHumanPuppets)
        {
            if (humanPuppet == null)
            {
                Debug.Log("É nulo");
                listOfHumanPuppets.Remove(humanPuppet);
                return;
            }
        }
    }


    void ChooseAttack()
    {
        if (!invokeHumanPuppetsOn)
        {
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = 0;
                Debug.Log(random);
                switch (random)
                {
                    case 0:
                        if(listOfHumanPuppets.Count < 5)
                        {
                            invokeHumanPuppetsOn = true;
                        }
                        timeToNextAttack = 3f;
                        break;

                    case 1:
                        timeToNextAttack = 3f;
                        break;
                }
            }
        }
    }
}
