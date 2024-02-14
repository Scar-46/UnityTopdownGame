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

    public float separationAngle = 0; // Angle of separation between projectiles
    public int projectiles = 1; // Number of projectiles per shoot


    public AttackState attackState;


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PerformAttack()
    {
        AudioManager.Instance.Play("MagicAttack");
        animator.SetTrigger("Attack");

        for (int i = 0; i < projectiles; i++)
        {
            // Instantiate proyectile.
            GameObject spell = Instantiate(projectile, projectileOrigin.position, Quaternion.identity);

            // Calculate player direction.
            Vector2 EnemyPos = transform.position;
            Vector2 TargetPos = player.transform.position;
            Vector2 direction = (TargetPos - EnemyPos).normalized;

            // Calculate rotation angle with separation.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += i * separationAngle;

            // Rotate proyectile.
            spell.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            spell.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * projectileForce;
            spell.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
        }
        yield return new WaitForSeconds(attackDelay);
        attackState.attacking = false;
    }
}
