
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy")
        {
            if (collision.tag == "Player")
            {
                PlayerStats.Instance.DealDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
