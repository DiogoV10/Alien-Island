using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{


    public static MeleeWeapon Instance { get; private set; }


    private List<Collider> enemies = new List<Collider>();
    private bool canHit = false;


    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit)
            return;

        bool hasHit = false;

        if (other.CompareTag("Enemy"))
        {
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (other == enemies[i])
                    {
                        hasHit = true;
                    }
                }
            }
            
            if (!hasHit) 
            { 
                enemies.Add(other);
                Debug.Log("Hit");
            }
        }
    }

    public void SetCanHit(bool hit)
    {
        canHit = hit;
    }
}
