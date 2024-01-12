using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyShooting : EnemyAttack
{
    public GameObject projectile;
    public float projectileForce;
    public Transform projectileOrigin;
    public float attackDelay;

    public AttackState attackState;


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PerformAttack()
    {
        animator.SetTrigger("Attack");

        // Instantiate proyectile.
        GameObject spell = Instantiate(projectile, projectileOrigin.position, Quaternion.identity);

        // Calculate player direction.
        Vector2 EnemyPos = transform.position;
        Vector2 TargetPos = player.transform.position;
        Vector2 direction = (TargetPos - EnemyPos).normalized;

        // Calculate rotation angle.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate proyectile.
        spell.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        spell.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
        yield return new WaitForSeconds(attackDelay);
        attackState.attacking = false;
    }
}
