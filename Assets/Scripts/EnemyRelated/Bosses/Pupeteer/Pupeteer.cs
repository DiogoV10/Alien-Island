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
    private bool invokeHumanPuppetsOn = false, pursuingPlayer = false;


    [Header("MelleeAttack")]
    PupeteerMeleeAttack melleeAttack;
    private bool melleeAtackOn = false;

    private void Start()
    {
        melleeAttack = GetComponent<PupeteerMeleeAttack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChooseAttack();
        InvokeHumanPuppets();
        MelleeAtack();
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

    void MelleeAtack()
    {
        if (melleeAtackOn)
        {
            melleeAttack.melleeAtackTime -= Time.deltaTime;
            if (!pursuingPlayer)
            {
                melleeAttack.PursuitPlayer();
                pursuingPlayer = true;
            }
            
            melleeAttack.AttackPlayer();
            if (melleeAttack.melleeAtackTime < 0.1f)
            {
                melleeAtackOn = false;
                melleeAttack.melleeAtackTime = 20f;
                return;
            }     
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


    void ChooseAttack()
    {
        if (!invokeHumanPuppetsOn && !melleeAtackOn)
        {
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = 1; //Random.Range(0, 2);
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
                        melleeAtackOn = true;
                        break;
                }
            }
        }
    }
}
