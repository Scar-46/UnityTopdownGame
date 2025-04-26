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
        animator.SetBool("Attack", true);
        float damage = Random.Range(minDamage, maxDamage);
        PlayerStats.Instance.DealDamage(damage);
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(attackDelay);
        attackState.attacking = false;

    }
}