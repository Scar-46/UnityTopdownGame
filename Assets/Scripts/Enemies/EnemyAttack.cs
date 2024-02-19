using System.Collections;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected GameObject player;

    public float minDamage;
    public float maxDamage;
    public float knockbackForce;
    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    public abstract IEnumerator PerformAttack();

}