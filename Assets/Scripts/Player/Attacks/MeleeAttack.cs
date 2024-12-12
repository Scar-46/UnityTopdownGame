using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : PlayerAttack
{

    public static MeleeAttack Instance;
    public bool isAttacking = false;
    public float radius = 0.2f;

    private void Awake()
    {
        MeleeAttack.Instance = this;
        knockbackForce = 0.1f;
    }

    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !attackBloked)
        {
            attackBloked = true;
        }
    }

    public void DealDamage() {

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(this.transform.position, radius))
        {
            if (collider.GetComponent<EnemyHealth>() != null)
            {
                float damage = Random.Range(minDamage, maxDamage);
                Vector2 knockback = (collider.transform.position - transform.position).normalized * knockbackForce;
                collider.GetComponent<EnemyHealth>().DealDamage(damage,knockback);
            }
        }
        StartCoroutine(DelayAttack());
    }
}
