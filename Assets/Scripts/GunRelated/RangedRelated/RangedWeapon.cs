using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedWeapon : MonoBehaviour
{
    public KeyCode shootKey;

    public GameObject bullet;

    public Transform muzzle;

    [Header("Ammo Info")]
    [SerializeField] public int magSize, currentAmmo;

    bool isReloading;

    [Header("Shoot info")]
    [SerializeField] float timeSinceLastShot, gunFireRate, reloadTime;

    void Start()
    {
        isReloading = false;
    }

    
    void Update()
    {   
        if(Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
        
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

    void Shoot()
    {
        Vector3 muzzlePosition = muzzle.position;

        RaycastHit hit;
        if(Physics.Raycast(muzzle.position, muzzle.right, out hit)) 
        {
            Debug.Log(hit.transform.name);
            GameObject currentBullet = Instantiate(bullet, hit.point, Quaternion.identity);
        }
        

        
    }

    public bool GunCanShoot() => !isReloading && timeSinceLastShot > 1f / (gunFireRate / 60f) ;

    void ResetShoot()
    {
        isReloading = true;
    }
}
