using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinderAttackTrigger : MonoBehaviour
{

    [SerializeField] int playerMask;
    [SerializeField] Minder minder;
    float timeUntilPhysics = 3f;
    bool collided = false;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided)
        {
            timeUntilPhysics -= Time.deltaTime;
            if(timeUntilPhysics < 0.1f)
            {
                collider.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            if (minder != null)
            {
                minder.GetComponent<Minder>().enabled = true;
            }
            else Destroy(this.gameObject);
        }
    }
}
