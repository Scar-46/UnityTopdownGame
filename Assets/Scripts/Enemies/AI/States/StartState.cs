using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    public State nextState;

    [SerializeField]
    private Animator _Animator;

    [SerializeField]
    private EnemyHealth _Health;

    public bool autoStart = true;
    public float introDuration = 1f;

    private NavMeshAgent agent;
    private bool introStarted = false;
    private bool introFinished = false;
    private float introTimer = 0.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Animator = GetComponent<Animator>();
    }

    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Health = GetComponent<EnemyHealth>();
        _Health.start = false;
    }

    public override State RunState()
    {
        if (!autoStart)
        {
            if (_Health.health < _Health.maxHealth)
            {
                _Animator.SetTrigger("Intro");
                _Health.enabled = false;
            }
            return nextState;
        } else
        {
            if (!introStarted)
            {
                introStarted = true;
                introTimer = 0f;
            }

            if (!introFinished)
            {
                introTimer += Time.deltaTime;
                if (introTimer >= introDuration)
                {
                    introFinished = true;
                    _Health.enabled = true;
                    _Health.start = true;
                    return nextState;
                }
                else
                {
                    return this;
                }
            }

            return nextState;
        }
       
    }
}
