using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupeteerAttackTrigger : MonoBehaviour
{
    [SerializeField] int playerMask;
    [SerializeField] Pupeteer pupeteer;
    [SerializeField] Grid grid;
    [SerializeField] PathFinding pathFinding;
    float timeUntilPhysics = 3f;
    bool collided = false;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        //grid = GetComponent<Grid>();
        //pathFinding = GetComponent<PathFinding>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided)
        {
            timeUntilPhysics -= Time.deltaTime;
            if (timeUntilPhysics < 0.1f)
            {
                collider.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            if (pupeteer != null)
            {
                grid.enabled = true;
                pathFinding.enabled = true;
                pupeteer.GetComponent<Pupeteer>().enabled = true;
                pupeteer.GetComponent<PupeteerMeleeAttack>().enabled = true;
                
            }
            else Destroy(this.gameObject);
        }
    }
}

