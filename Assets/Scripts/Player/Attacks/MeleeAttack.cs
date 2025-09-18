using System.Collections;
using UnityEngine;

public class MeleeAttack : PlayerAttack
{
    public static MeleeAttack Instance;

    [Header("Combo Settings")]
    public string[] attackAnimations = { "MeleeAttack1", "MeleeAttack2"};
    public float comboResetTime = 1.0f;
    public float bufferTime = 0.2f; // how long input is stored
    public float radius = 0.2f;

    private int currentComboStep = 0;
    private float lastAttackTime;

    private bool bufferedInput = false;
    private float bufferTimer = 0f;

    private void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!attackBlocked)
                PerformAttack(); // free to attack now
            else
                BufferInput();   // store the press for later
        }

        // Reset combo if player waited too long
        if (Time.time - lastAttackTime > comboResetTime)
            currentComboStep = 0;

        // Count down buffer
        if (bufferedInput)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
                bufferedInput = false;
        }
    }

    private void PerformAttack()
    {
        attackBlocked = true;
        lastAttackTime = Time.time;

        // Play correct attack animation
        string animName = attackAnimations[currentComboStep];
        animator.Play(animName);
        AudioManager.Instance.Play("Miss");

        // Increment combo step
        currentComboStep = (currentComboStep + 1) % attackAnimations.Length;
    }

    private void BufferInput()
    {
        bufferedInput = true;
        bufferTimer = bufferTime;
    }

    // Called by Animation Event at impact frame
    public void DealDamage()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            if (collider.TryGetComponent(out EnemyHealth enemy))
            {
                float damage = Random.Range(minDamage, maxDamage);
                Vector2 knockback = (collider.transform.position - transform.position).normalized * knockbackForce;
                enemy.DealDamage(damage, knockback);

                AudioManager.Instance.Stop("Miss");
                AudioManager.Instance.Play("Attack");
            }
        }
    }

    // Called by Animation Event at the end of the attack animation
    public void EndAttack()
    {
        attackBlocked = false;

        // If player pressed attack during animation → trigger next combo
        if (bufferedInput)
        {
            bufferedInput = false;
            PerformAttack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
