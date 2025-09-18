using UnityEngine;
using UnityEngine.AI;

public class StunState : State
{
    [SerializeField] private float stunDuration = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private float stunTimer;

    private EnemyHealth enemyHealth;
    private AIData aiData;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        aiData = GetComponent<AIData>();
    }

    public override void OnEnter()
    {
        _isFacingRight = transform.localScale.x > 0;
        stunTimer = stunDuration;
        agent.ResetPath();
        animator.SetFloat("Speed", 0f);

        if (enemyHealth != null)
            enemyHealth.invincible = true;
    }

    public override void OnExit()
    {
        if (enemyHealth != null)
            enemyHealth.invincible = false;
    }

    public override State RunState()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0f)
        {
            return GetComponent<RoamingState>();
        }

        return this;
    }
}
