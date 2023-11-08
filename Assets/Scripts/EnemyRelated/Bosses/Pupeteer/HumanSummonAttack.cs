using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSummonAttack : MonoBehaviour, IEntity
{
    [Header("Player Reference")]
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealthManager playerHealthManager;
    [SerializeField] LayerMask whatIsPlayer;

    [Header("HumanPuppetSO reference")]
    [SerializeField] HumanPuppetSO humanPuppet;

    private bool isPlayerInSightRange, isPlayerInAttackRange, alreadyAttacked;
   

    System.Action<GameObject> callback;

    void FixedUpdate()
    {
        SearchForPlayer();
        FollowPlayer();
    }

    void SearchForPlayer()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, humanPuppet.attackRange, whatIsPlayer);
    }

    void FollowPlayer()
    {
        if (!isPlayerInAttackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, humanPuppet.speed * Time.deltaTime);
        }
        if (isPlayerInAttackRange) AttackPlayer();
    }

    void AttackPlayer()
    {
        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            IEntity entity = player.GetComponent<IEntity>();
            entity.TakeDamage(humanPuppet.damageAmount);
            Debug.Log("Attacked!");
            Debug.Log(playerHealthManager.playerHealth);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), humanPuppet.timeBetweenAttacks);
        }
    }

    private void ResetAttack() => alreadyAttacked = false;

    public void TakeDamage(float damage)
    {
        humanPuppet.hp -= damage;
        Debug.Log(humanPuppet.hp);
        if (humanPuppet.hp <= 0f) Die();
    }

    public void Die()
    {
        Debug.Log("Enemy Died!");
        OnEntityDeath();
    }

    public void OnEntityDeath()
    {
        callback(gameObject);
        //Destroy(gameObject);
    }

    public void SetCallback(System.Action<GameObject> _callback)
    {
        callback = _callback;
    }

    public void SetPlayer (GameObject _player)
    {
        player = _player;
        playerHealthManager = player.GetComponent<PlayerHealthManager>();
    }
}
