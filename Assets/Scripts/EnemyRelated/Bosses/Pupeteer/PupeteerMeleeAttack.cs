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
    Animator animator;

    private bool alreadyAttacked, isPlayerInAttackRange, pursuingPlayer = false;
    [SerializeField] public float melleeAtackTime = 20f;

    System.Action<GameObject> callback;

    // Start is called before the first frame update
    void Start()
    {
        //pathToPlayer = GetComponent<Grid>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SearchForPlayer();
    }

    IEnumerator MoveToPlayer()
    {
        List<Node> enemypath = pathToPlayer.path;
        int i = 0;
        while (pursuingPlayer)
        {
            Debug.Log("Follow Player");
            if(enemypath != pathToPlayer.path && !isPlayerInAttackRange)
            {
                Debug.Log("enemy path: " + enemypath);
                enemypath = pathToPlayer.path;
                i = 0;
            }
            if(i < pathToPlayer.path.Count)
            {
                transform.LookAt(player.transform);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(enemypath[i].worldPos.x, transform.position.y, enemypath[i].worldPos.z), speed * Time.deltaTime);
                i++;
            }
            
            yield return null;
        }
    }

    void PursuitPlayer()
    {
        StartCoroutine(MoveToPlayer());
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, pupeteerSO.attackRange, whatIsPlayer);
    }

    void AttackPlayer()
    {
        if (isPlayerInAttackRange)
        {
            pursuingPlayer = false;
            animator.SetBool("isAttacking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", true);
        }
        
    }

    void Attack()//esta a ser chamdo em evento de animação
    {
        transform.LookAt(player.transform);
        if (!alreadyAttacked)
        {
            IEntity entity = player.GetComponent<IEntity>();
            entity.TakeDamage(pupeteerSO.damageAmount);
            Debug.Log("Attacked!");
            Debug.Log(playerHealthManager.GetHealth());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), pupeteerSO.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public void DoAttack()
    {
        melleeAtackTime -= Time.deltaTime;
        if (!pursuingPlayer && !isPlayerInAttackRange)
        {
            Debug.Log("Pursuing Player");
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
            pursuingPlayer = true;
            PursuitPlayer();
        }

        AttackPlayer();

        if (melleeAtackTime < 0.05f)
        {
            pursuingPlayer = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }

    public void TakeDamage(float damage)
    {
        pupeteerSO.hp -= damage;
        Debug.Log(pupeteerSO.hp);
        if (pupeteerSO.hp <= 0f)
        {
            MainQuests.Instance.QuestEnd();
            SkillPoints.Instance.IncreaseSkillPoints();
            Die();
        }
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
