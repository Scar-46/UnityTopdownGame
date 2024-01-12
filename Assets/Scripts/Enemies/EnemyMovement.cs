using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool _isFacingRight = true;

    private Animator _Animator;

    [SerializeField]
    NavMeshAgent agent;

    public void Start()
    {
         rb2d = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void Update()
    {
        _Animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public void MoveEnemy(Vector2 target)
    {
        agent.SetDestination(target);
        _Animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    public bool followingTarget()
    {
        return agent.hasPath || agent.velocity.sqrMagnitude > 0f;
    }

    public void Flip(Vector2 target)
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
