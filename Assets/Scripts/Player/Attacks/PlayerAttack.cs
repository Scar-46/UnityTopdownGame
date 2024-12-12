using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerAttack : MonoBehaviour
{
    [Header("Damage settings")]
    public float minDamage;
    public float maxDamage;
    public float knockbackForce = 16;

    public bool attackBloked = false;
    public float attackDelay = 0f;

    [Header("Animator")]
    public Animator animator;

    protected IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBloked = false;
    }
}
