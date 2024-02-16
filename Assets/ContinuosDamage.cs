using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuosDamage : MonoBehaviour
{
    //Attack Damage
    public float minDamage;
    public float maxDamage;
    public float knockbackForce = 0.1f;

    //Attack Delay
    private bool attackBloked = false;
    public float attackDelay = 0f;

    //Mana consumption
    public float manaConsumption = 0;
    public float consumptionRate = 0;
    private bool consume = true;


    private void Update()
    {
        if (consume)
        {
            consume = false;
            PlayerStats.Instance.UseMagic(manaConsumption);
            StartCoroutine(DelayConsume());
        }
        if (PlayerStats.Instance.magic <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !attackBloked)
        {
            attackBloked = true;
            float damage = Random.Range(minDamage, maxDamage);
            Vector2 knockback = (collision.transform.position - transform.position).normalized * knockbackForce;
            collision.GetComponent<EnemyHealth>().DealDamage(damage, knockback);
            StartCoroutine(DelayAttack());
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBloked = false;
    }

    private IEnumerator DelayConsume()
    {
        yield return new WaitForSeconds(consumptionRate);
        consume = true;
    }
}
