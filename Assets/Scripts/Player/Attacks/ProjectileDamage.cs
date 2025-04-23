using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float damage;
    public int heal = 0;
    public Vector2 knockback;
    public float knockbackforce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            if(collision.GetComponent<EnemyHealth>() != null)
            {
                knockback = (collision.transform.position - transform.position).normalized * knockbackforce;
                Debug.Log(knockback);
                collision.GetComponent<EnemyHealth>().DealDamage(damage, knockback);
                PlayerStats.Instance.HealCharacter(heal);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
