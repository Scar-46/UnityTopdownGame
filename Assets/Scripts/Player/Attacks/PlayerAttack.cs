using System.Collections;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] protected float minDamage = 5f;
    [SerializeField] protected float maxDamage = 10f;
    [SerializeField] protected float knockbackForce = 16f;

    [Header("Attack Control")]
    [SerializeField] protected float attackDelay = 0.3f;
    public bool attackBlocked = false;

    [Header("Animator")]
    protected Animator animator;

    protected virtual void Awake()
    {
        // Always grab animator from player root
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        animator = playerObj.GetComponent<Animator>();
    }

    protected IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }

    protected float GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}
