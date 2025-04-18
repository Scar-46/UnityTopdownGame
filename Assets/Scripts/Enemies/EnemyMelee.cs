using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyAttack
{
    public float attackDelay;

    public AttackState attackState;

    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator PerformAttack()
    {
        AudioManager.Instance.Play("EnemyAttack");
        animator.SetTrigger("Attack");
        float damage = Random.Range(minDamage, maxDamage);
        PlayerStats.Instance.DealDamage(damage);

        yield return new WaitForSeconds(attackDelay);
        attackState.attacking = false;
    }
}