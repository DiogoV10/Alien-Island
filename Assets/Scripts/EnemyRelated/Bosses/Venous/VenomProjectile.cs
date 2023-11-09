using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomProjectile : MonoBehaviour
{

    [SerializeField] VenomProjectileAttack venomProjectileAttack;
    Rigidbody rigidBody;
    Vector3 distToPlayer;
    float speed = 20f, lifeTime = 3f;
    bool colided = false, startedMoving = false;

    [Header("Player Reference")]
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] int playerMask;
    Transform target;

    [Header("Attributes")]
    [SerializeField] VenousSO venousSO;

    // Start is called before the first frame update
    void Start()
    {
        startedMoving = false;
        rigidBody = GetComponent<Rigidbody>();
        //distToPlayer = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        //MoveToPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedMoving)
        {
            DistanceToPlayer();
            MoveToPlayer();
            startedMoving = true;
        }
        DestroyIfNotCollide();
    }

    void MoveToPlayer()
    {
        rigidBody.AddForce(distToPlayer, ForceMode.Impulse);
    }

    void DestroyIfNotCollide()
    {
        if (!colided)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0.1f) Destroy(gameObject);
        }
    }

    void DistanceToPlayer()
    {
        distToPlayer = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
    }

    void DamagePlayer()
    {
        IEntity entity = target.GetComponent<IEntity>();
        entity.TakeDamage(venousSO.VenomProjectileAttack);
        Debug.Log(playerHealthManager.playerHealth);
    }

    public void SetTarget(Transform _target) 
    {
        target = _target;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerMask)
        {
            if (venomProjectileAttack.attackOn == false)
            {
                Debug.Log("Ataque Ativado");
                venomProjectileAttack.attackOn = true;
                Destroy(gameObject);
                return;
            }

            else
            {
                DamagePlayer();
                Destroy(gameObject);
            }
        }
        

        
    }
}
