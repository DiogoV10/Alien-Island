using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomCloudAttack : MonoBehaviour
{

    [SerializeField] float venousDamageTime = 1f;
    public float venousEffectTime, attackTime = 20f; 
    [SerializeField] List<GameObject> groupParticles;
    [SerializeField] List<GameObject> groupVenomExit;
    private bool stopActivation = false, isIluminated = false, spaceChosen = false, isInVenom = false;
    int random = 0;

    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("VenousSO reference")]
    [SerializeField] VenousSO venousSO; 

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject particle in groupParticles)
        {
            particle.SetActive(false);
        } 
    }

    public void StartAttack()
    {
        attackTime -= Time.deltaTime;
        if (isInVenom)
        {
            venousDamageTime -= Time.deltaTime;
            venousEffectTime -= Time.deltaTime;
            if (venousEffectTime < .05f) isInVenom = false;
        }

        if (!spaceChosen)
        {
            random = Random.Range(0, 4);
            spaceChosen = true;
        }

        if (attackTime > 15 && !isIluminated)
        {
            IluminateSpots(random);
            isIluminated = true;
        }

         else if (attackTime < 15f && attackTime > .1f)
        {
            if (!stopActivation)
            {
                TurnOffSpots();
                ActivateVenom(random);
                stopActivation = true;
            }

            if (!isInVenom)
            {
                VerifyPlayerDamage();
            }

            if (isInVenom && venousEffectTime > 0.1f)
            {
                DamagePlayer();
            }
        }

        else if (attackTime < .1f && venousEffectTime < .1f)
        {
            DeactivateVenom();
            stopActivation = false;
            spaceChosen = false;
            isIluminated = false;
            attackTime = 20f;
        }
    }

    void ActivateVenom(int random)
    {
        for (int i = 0; i < groupParticles.Count; i++)
        {
            if (i == random) continue;
            groupParticles[i].SetActive(true);
        }
    }
    void DeactivateVenom()
    {
        foreach (GameObject particle in groupParticles)
        {
            particle.SetActive(false);
        }
    }

    void VerifyPlayerDamage()
    {
        for (int i = 0; i < groupVenomExit.Count; i++)
        {
            if (i == random) continue;
            foreach (Transform exit in groupVenomExit[i].transform)
            {
                isInVenom = Physics.CheckSphere(exit.transform.position, 1f, whatIsPlayer);
                if (isInVenom)
                {
                    venousDamageTime = 1.5f;
                    venousEffectTime = 5f;
                    return;
                }
                
            }
        }
    }

    void IluminateSpots(int random)
    {
        for (int i = 0; i < groupVenomExit.Count; i++)
        {
            if (i == random) continue;
            foreach (Transform item in groupVenomExit[i].transform)
            {
                item.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    void TurnOffSpots()
    {
        foreach (GameObject particleExitGroup in groupVenomExit)
        {
            foreach(Transform particleExit in particleExitGroup.transform)
                particleExit.transform.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
    } 

    void DamagePlayer()
    {
        if(venousDamageTime < .1f)
        {
            venousDamageTime = 1.5f;
            IEntity entity = player.GetComponent<IEntity>();
            entity.TakeDamage(venousSO.venomSmokeAttack);
            Debug.Log(playerHealthManager.playerHealth);
        }
    }
}

