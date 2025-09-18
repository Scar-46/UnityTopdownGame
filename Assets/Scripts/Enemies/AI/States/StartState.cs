using System;
using UnityEngine;
using UnityEngine.AI;

public class StartState : State
{
    [SerializeField] private AttackState attackState;
    [SerializeField] private State nextState;

    private Animator animator;
    private EnemyHealth health;
    private NavMeshAgent agent;
    private AIData aiData;

    public bool autoStart = true;
    public float introDuration = 1f;

    private bool introStarted = false;
    private bool introFinished = false;
    private float introTimer = 0f;

    public override void OnEnter()
    {
        _isFacingRight = transform.localScale.x > 0;
        if (animator == null)
            animator = GetComponent<Animator>();

        if (health == null)
            health = GetComponent<EnemyHealth>();

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        if (aiData == null)
            aiData = GetComponent<AIData>();

        // Reset intro sequence
        introStarted = false;
        introFinished = false;
        introTimer = 0f;

        // Prevent damage until intro ends
        health.start = false;
        health.enabled = false;

        // Stay in this state until ready
        nextState = this;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
        aiData = GetComponent<AIData>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        health.start = false;
        nextState = this;
    }

    public override void OnExit()
    {
        // Ensure health is active when leaving
        health.enabled = true;
        health.start = true;
    }

    public override State RunState()
    {
        if (!autoStart)
        {
            // Trigger intro only when enemy takes damage
            if (health.health < health.maxHealth && !introStarted)
            {
                animator.SetTrigger("Intro");
                introStarted = true;
                introTimer = 0f;
            }

            if (introStarted && !introFinished)
            {
                introTimer += Time.deltaTime;
                if (introTimer >= introDuration)
                {
                    introFinished = true;
                    return attackState;
                }
            }

            return nextState;
        }
        else
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
                    return attackState;
                }
                else
                {
                    return nextState;
                }
            }

            return nextState;
        }
    }
}
