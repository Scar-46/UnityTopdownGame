using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    [SerializeField]
    TargetDetector targetDetector;

    [SerializeField]
    AIData aiData;

    NavMeshAgent agent;

    Animator _Animator;

    //States
    [SerializeField]
    RoamingState _RoamingState;

    [SerializeField]
    AttackState _AttackState;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Animator = GetComponent<Animator>();
    }

    public override State RunState()
    {
        agent.stoppingDistance = _AttackState._AttackDistance;
        targetDetector.Detect(aiData);

        if (aiData.targets != null && aiData.targets.Count > 0)
        {
            float distance = Vector2.Distance(aiData.targets[0].position, transform.position);

            if (distance <= _AttackState._AttackDistance)
            {
                _AttackState._isFacingRight = _isFacingRight;
                _AttackState.attacking = false;
                return _AttackState;
            }
            else
            {
                Flip(aiData.targets[0]);
                agent.SetDestination(aiData.targets[0].position);
                _Animator.SetFloat("Speed", 0.06f);
            }
            return this;
        }
        else
        {
            _Animator.SetFloat("Speed", 0.04f);
            _RoamingState._isFacingRight = _isFacingRight;
            return _RoamingState;
        }
    }

    public void Flip(Transform target)
    {
        if (target.position.x > this.transform.position.x && !_isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
        else if (target.position.x < this.transform.position.x && _isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
    }
}