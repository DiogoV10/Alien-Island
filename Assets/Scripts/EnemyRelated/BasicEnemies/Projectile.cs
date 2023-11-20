using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    
    private float effectAmount = 0f;
    private bool shouldMove = false;

    private void Update()
    {
        if (shouldMove)
        {
            transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthManager>().TakeDamage(effectAmount);

            Destroy(gameObject);
        }
    }

    public void SetDamageAndMove(float damageAmount, Vector3 direction)
    {
        effectAmount = damageAmount;
        transform.forward = direction.normalized;
        shouldMove = true;
    }
}
