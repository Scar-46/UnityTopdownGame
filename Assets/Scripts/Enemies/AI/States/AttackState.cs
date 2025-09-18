using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    [SerializeField] private EnemyAttack enemyAttack;

    [SerializeField] public float _AttackDistance = 0.5f;
    [SerializeField] public float _FleeDistance = 0f;

    [SerializeField] private RoamingState roamingState;
    [SerializeField] private ChaseState chasingState;

    [SerializeField] private TargetDetector targetDetector;
    [SerializeField] private AIData aiData;

    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Animator animator;

    public bool attacking = false;

    public override void OnEnter()
    {
        _isFacingRight = transform.localScale.x > 0;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        if (enemyAttack == null)
            enemyAttack = GetComponent<EnemyAttack>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        // Stop movement before attacking
        rb.velocity = Vector2.zero;
        agent.ResetPath();

        attacking = false;
        animator.SetFloat("Speed", 0f);
    }

    public override void OnExit()
    {
        // Reset attack state when leaving
        attacking = false;
        animator.SetFloat("Speed", 0f);
    }

    public override State RunState()
    {
        State nextState = this;

        // Prevent starting multiple attacks in a single frame
        if (!attacking)
        {
            attacking = true;

            // Detect player
            targetDetector.Detect(aiData);

            if (aiData.targets != null && aiData.targets.Count > 0)
            {
                Transform player = aiData.targets[0].transform;
                Flip(player.position);
                float distance = Vector2.Distance(player.position, transform.position);

                if (distance <= _FleeDistance)
                {
                    // Flee from player
                    Vector3 playerDir = transform.position - player.position;
                    Vector3 newPos = transform.position + playerDir;

                    agent.stoppingDistance = 0;
                    agent.SetDestination(newPos);

                    StartCoroutine(enemyAttack.PerformAttack());
                }
                else if (distance <= _AttackDistance)
                {
                    // Normal attack
                    StartCoroutine(enemyAttack.PerformAttack());
                }
                else
                {
                    // Out of range → chase again
                    nextState = chasingState;
                }
            }
            else
            {
                // No targets → back to roaming
                nextState = roamingState;
            }
        }

        return nextState;
    }
}
