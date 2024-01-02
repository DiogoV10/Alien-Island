using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCollider: MonoBehaviour
{

    int playerMask = 3;
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerMask)
        {
            particle.enableEmission = false;
        }
    }
}
