using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    [SerializeField] private TargetDetector targetDetector;
    [SerializeField] private AIData aiData;

    private NavMeshAgent agent;
    private Animator animator;

    // States
    [SerializeField] private RoamingState roamingState;
    [SerializeField] private AttackState attackState;

    public override void OnEnter()
    {
        _isFacingRight = transform.localScale.x > 0;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        agent.stoppingDistance = attackState._AttackDistance;
        animator.SetFloat("Speed", 0f);
    }

    public override void OnExit()
    {
        // Reset movement when leaving chase
        agent.ResetPath();
        animator.SetFloat("Speed", 0f);
    }

    public override State RunState()
    {
        State nextState = this;

        // Always check targets
        targetDetector.Detect(aiData);

        if (aiData.targets != null && aiData.targets.Count > 0)
        {
            Flip(aiData.targets[0]);
            float distance = Vector2.Distance(aiData.targets[0].position, transform.position);

            if (distance <= attackState._AttackDistance)
            {
                attackState._isFacingRight = _isFacingRight;
                attackState.attacking = false;
                nextState = attackState;
            }
            else
            {
                agent.SetDestination(aiData.targets[0].position);
                animator.SetFloat("Speed", 0.06f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0.04f);
            roamingState._isFacingRight = _isFacingRight;
            nextState = roamingState;
        }

        return nextState;
    }


}
