using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedWeapon : MonoBehaviour
{


    public static RangedWeapon Instance { get; private set; }

    [SerializeField] private KeyCode shootKey;

    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform muzzle;


    [Header("Ammo Info")]
    [SerializeField] private int magSize, currentAmmo;

    private bool isReloading;

    [Header("Shoot info")]
    [SerializeField] float timeSinceLastShot, gunFireRate, reloadTime;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isReloading = false;
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
        Vector3 muzzlePosition = muzzle.position;

        Debug.Log("Shoot");

        RaycastHit hit;
        if(Physics.Raycast(muzzle.position, muzzle.right, out hit)) 
        {
            Debug.Log(hit.transform.name);
            //GameObject currentBullet = Instantiate(bullet, hit.point, Quaternion.identity);
            if (hit.transform.gameObject.CompareTag("Enemy") )
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    public bool GunCanShoot() => !isReloading && timeSinceLastShot > 1f / (gunFireRate / 60f) ;

    void ResetShoot()
    {
        isReloading = true;
    }
}
