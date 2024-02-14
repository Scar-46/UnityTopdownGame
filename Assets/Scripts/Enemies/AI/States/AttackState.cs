using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    [SerializeField]
    private EnemyAttack _Attack;

    [SerializeField]
    public float _AttackDistance = 0.5f;

    [SerializeField]
    public float _FleeDistance = 0;

    [SerializeField]
    private State roamingState;

    [SerializeField]
    private ChaseState chasingState;

    Rigidbody2D _Rigidbody2;

    [SerializeField]
    private Detector targetDetector;

    [SerializeField]
    private AIData aiData;

    NavMeshAgent agent;

    public bool attacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _Attack = GetComponent<EnemyAttack>();
        _Rigidbody2 = GetComponent<Rigidbody2D>();
    }

    private void PerformDetection()
    {
        targetDetector.Detect(aiData);
    }

    public override State RunState()
    {
        if (attacking == false)
        {
            attacking = true;
            _Rigidbody2.velocity = Vector2.zero;
            PerformDetection();

            if (aiData.targets != null && aiData.targets.Count > 0)
            {
                Transform player = aiData.targets[0].transform;
                Flip(player.position);
                float distance = Vector2.Distance(player.position, transform.position);

                if (distance <= _FleeDistance)
                {
                    Vector3 playerDir = transform.position - player.position;
                    Vector3 newPos = transform.position + playerDir;
                    agent.stoppingDistance = 0;
                    agent.SetDestination(newPos);
                    StartCoroutine(_Attack.PerformAttack());
                    return this;

                }
                else if (distance <= _AttackDistance)
                {
                    StartCoroutine(_Attack.PerformAttack());
                    return this;
                }
                else
                {
                    chasingState._isFacingRight = _isFacingRight;
                    return chasingState;
                }
            }
            else
            {
                roamingState._isFacingRight = _isFacingRight;
                return roamingState;
            }
        }
        else
        {
            return this;
        }
    }
    public void Flip(Vector3 target)
    {
        if (target.x > this.transform.position.x && !_isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
        else if (target.x < this.transform.position.x && _isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            this.transform.Rotate(0, 180, 0);
        }
    }
}
