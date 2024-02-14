using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMeleeRange : EnemyAttack
{
    public float attackDelay;

    public AttackState attackState;

    //Melee
    public float secondAttackDistance;
    public float dashSpeed;
    public float _dashCooldown;
    private bool _canDash = true;

    //Range
    public float firstAttackDistance;
    public GameObject projectile;
    public Transform projectileOrigin;

    public int projectileNumber;
    public float projectileForce;
    public float projectileRotation;
    public float spinSpeed = 50f;

    public float angleChangeAmount = 2;
    private float intialAngle;

    public int rangeBurst = 6;
    public int rangeBurstCounter = 0;
    private int timer = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PerformAttack()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance <= secondAttackDistance)
        {
            AudioManager.Instance.Play("Attack");
            animator.SetTrigger("Attack");
            float damage = Random.Range(minDamage, maxDamage);
            PlayerStats.Instance.DealDamage(damage);
            yield return new WaitForSeconds(attackDelay);
        }
        else
        {
            if (rangeBurstCounter < rangeBurst)
            {
                animator.SetTrigger("Attack");

                for (int i = 0; i < projectileNumber; i++)
                {
                    float angle = intialAngle + i * projectileRotation;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    var spell = Instantiate(projectile, gameObject.transform.position, rotation);

                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                    float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
                    spell.GetComponent<EnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
                }
                intialAngle += angleChangeAmount;
                yield return new WaitForSeconds(attackDelay);
                rangeBurstCounter++;
            }
            else if (_canDash)
            {
                // Calculate player direction.
                _canDash = false;
                Vector2 EnemyPos = transform.position;
                Vector2 TargetPos = player.transform.position;
                Vector2 direction = (TargetPos - EnemyPos).normalized;
                gameObject.GetComponent<Rigidbody2D>().velocity = direction * dashSpeed;
                attackState._AttackDistance = secondAttackDistance;
                yield return new WaitForSeconds(_dashCooldown);
                _canDash = true;
                StartCoroutine(ResetRangeBurstCounter());
            }
        }
        attackState.attacking = false;
    }

    private IEnumerator ResetRangeBurstCounter()
    {
        yield return new WaitForSeconds(4f);
        rangeBurstCounter = 0;
        attackState._AttackDistance = firstAttackDistance;
    }
}
