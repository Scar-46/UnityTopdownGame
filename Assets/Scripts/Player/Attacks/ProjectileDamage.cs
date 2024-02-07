using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float damage;
    public Vector2 knockback;
    public float knockbackforce = 0.001f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "Player")
        {
            if(collision.GetComponent<EnemyHealth>() != null)
            {
                knockback = (collision.transform.position - transform.position).normalized * knockbackforce;
                collision.GetComponent<EnemyHealth>().DealDamage(damage, knockback);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name != "Player")
        {
            Destroy(gameObject);
        }
    }
}
