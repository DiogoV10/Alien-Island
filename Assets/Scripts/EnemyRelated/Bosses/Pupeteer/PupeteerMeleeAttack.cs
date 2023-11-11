using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupeteerMeleeAttack : MonoBehaviour, IEntity
{
    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("PupetteerSO reference")]
    [SerializeField] PupeteerSO pupeteerSO;

    public Grid pathToPlayer;
    float speed = 10f;
    Rigidbody rigidBody;

    private bool alreadyAttacked, isPlayerInAttackRange;
    [SerializeField] public float melleeAtackTime = 20f;

    System.Action<GameObject> callback;

    // Start is called before the first frame update
    void Start()
    {
        //pathToPlayer = GetComponent<Grid>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SearchForPlayer();
        //AttackPlayer();
        //Debug.Log(isPlayerInAttackRange);
        //StartCoroutine(MoveToPlayerTwo());
    }

    IEnumerator MoveToPlayer()
    {
        List<Node> enemypath = pathToPlayer.path;
        int i = 0;
        while (melleeAtackTime >= 0.1f)
        {
            Debug.Log("Follow Player");
            if(enemypath != pathToPlayer.path && !isPlayerInAttackRange)
            {
                enemypath = pathToPlayer.path;
                i = 0;
            }
            //rigidBody.MovePosition(enemypath[i].worldPos);
            if(i <= pathToPlayer.path.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(enemypath[i].worldPos.x, enemypath[i].worldPos.y + 2.5f, enemypath[i].worldPos.z), speed * Time.deltaTime);
                i++;
            }
            
            yield return null;
        }
    }

    public void PursuitPlayer()
    {
        StartCoroutine(MoveToPlayer());
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, pupeteerSO.attackRange, whatIsPlayer);
    }

    public void AttackPlayer()
    {
        if (isPlayerInAttackRange) Attack();
    }

    void Attack()
    {
        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            IEntity entity = player.GetComponent<IEntity>();
            entity.TakeDamage(pupeteerSO.damageAmount);
            Debug.Log("Attacked!");
            Debug.Log(playerHealthManager.playerHealth);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), pupeteerSO.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public void TakeDamage(float damage)
    {
        pupeteerSO.hp -= damage;
        Debug.Log(pupeteerSO.hp);
        if (pupeteerSO.hp <= 0f) Die();
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        //callback(gameObject);
        Destroy(gameObject);
    }

    public void SetCallback(System.Action<GameObject> _callback)
    {
        callback = _callback;
    }

    public void SetPlayer(GameObject _player)
    {
        player = _player;
        playerHealthManager = player.GetComponent<PlayerHealthManager>();
    }
}
