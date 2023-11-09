using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedWeapon : MonoBehaviour
{


    public static RangedWeapon Instance { get; private set; }

    [SerializeField] private Transform muzzle;


    //[Header("Ammo Info")]
    //[SerializeField] private int magSize, currentAmmo;

    //private bool isReloading;

    [Header("Shooting Info")]
    [SerializeField] private float gunDamage;

    private void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {   
        //if(Input.GetKeyDown(shootKey))
        //{
        //    Shoot();
        //}
        
        /*
        timeSinceLastShot += Time.deltaTime;
        if (GunCanShoot() && currentAmmo > 0)
        {
            if (!gameObject.activeSelf) return;

            Shoot();
            Debug.Log("Pew Pew");
        }
        */
    }

    public void Shoot()
    {
        //Debug.Log("Shoot");

        //RaycastHit hit;
        //if(Physics.Raycast(muzzle.position, muzzle.right, out hit)) 
        //{
        //    //Debug.Log(hit.transform.name);
            
        //    if (hit.transform.gameObject.CompareTag("Enemy") )
        //    {
        //        //Destroy(hit.transform.gameObject);
        //        IEntity entity = hit.collider.GetComponent<IEntity>();
        //        if (entity != null)
        //        {
        //            entity.TakeDamage(gunDamage);
        //        }
        //    }
        //}
    }

    public float GunDamage()
    {
        return gunDamage;
    }
}
