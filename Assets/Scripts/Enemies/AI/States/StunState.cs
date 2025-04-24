using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class StunState : State
{
    [SerializeField] float stunDuration = 2f;
    private float stunTimer;

    NavMeshAgent agent;
    Animator _Animator;

    // Reference to a fallback state after stun (like Roaming or Idle)
    [SerializeField] RoamingState _RoamingState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        stunTimer = 0f;
        if (agent != null)
            agent.ResetPath();

        if (_Animator != null)
            _Animator.SetFloat("Speed", 0f);
    }

    public override State RunState()
    {
        stunTimer += Time.deltaTime;

        if (stunTimer >= stunDuration)
        {
            return _RoamingState;
        }

        return this;
    }
}
