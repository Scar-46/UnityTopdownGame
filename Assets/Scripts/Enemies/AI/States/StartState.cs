using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    private State nextState;

    [SerializeField]
    private AttackState attackState;

    [SerializeField]
    private Animator _Animator;

    [SerializeField]
    private EnemyHealth _Health;

    NavMeshAgent agent;


    public override State RunState()
    {
       if(_Health.health < _Health.maxHealth)
        {
            _Animator.SetTrigger("Intro");
            _Health.enabled = false;
        }
        return nextState;
    }

    public void changeState()
    {
        nextState = attackState;
        _Health.enabled = true;
        _Health.start = true;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Health = GetComponent<EnemyHealth>();
        _Health.start = false;
        nextState = this;
    }

}
