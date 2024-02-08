using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public static MeleeAttack Instance;

    public float minDamage;
    public float maxDamage;

    public Animator animator;

    public bool isAttacking = false;
    public bool attackBloked = false;
    public float attackDelay = 0f;
    public float radius = 0.2f;
    public float knockbackForce = 0.1f;

    private void Awake()
    {
        MeleeAttack.Instance = this;
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
                Debug.Log(knockback);
                collider.GetComponent<EnemyHealth>().DealDamage(damage,knockback);
            }
        }
        StartCoroutine(DelayAttack());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = this.transform == null ? Vector3.zero : this.transform.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBloked = false;
    }
}
