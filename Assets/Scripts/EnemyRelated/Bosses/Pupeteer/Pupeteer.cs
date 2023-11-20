using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pupeteer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    [SerializeField] private float timeToNextAttack = 3f;
    Animator animator;

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
        animator = GetComponent<Animator>();
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
        if (invokeHumanPuppetsOn && listOfHumanPuppets.Count < 4)
        {
            transform.LookAt(player.transform);
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
                float humanPupetPosition = Random.Range(-5, 5);
                GameObject go = Instantiate(humanPuppet, new Vector3(transform.position.x + humanPupetPosition, 1, transform.position.z + humanPupetPosition), Quaternion.identity);
                go.GetComponent<HumanSummonAttack>().SetPlayer(player);
                go.GetComponent<HumanSummonAttack>().SetCallback(RemoveFromEnemiesList);
                listOfHumanPuppets.Add(go);
                timeBetweenPuppets = 4f;
            }
        }
        
        else
        {
            invokeHumanPuppetsOn = false;
            animator.SetBool("isHumanPupetAttack", false);
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
                animator.SetBool("isRunning", true);
                animator.SetBool("isAttacking", false);
                melleeAttack.PursuitPlayer();
                pursuingPlayer = true;
            }
            
            melleeAttack.AttackPlayer();
            if (melleeAttack.melleeAtackTime < 0.05f)
            {
                melleeAtackOn = false;
                pursuingPlayer = false;
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
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
            transform.LookAt(player.transform);
            timeToNextAttack -= Time.deltaTime;
            if (timeToNextAttack < 0f)
            {
                int random = Random.Range(0, 2);
                Debug.Log(random);
                switch (random)
                {
                    case 0:
                        if(listOfHumanPuppets.Count < 4)
                        {
                            animator.SetBool("isHumanPupetAttack", true);
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
