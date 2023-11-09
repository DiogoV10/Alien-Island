using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinderAttackTrigger : MonoBehaviour
{

    [SerializeField] int playerMask;
    [SerializeField] Minder minder;
    Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            minder.GetComponent<Minder>().enabled = true;
            collider.isTrigger = false;

        }
    }
}
