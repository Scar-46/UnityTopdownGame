using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float distance;
    private GameObject _player;
    private bool _isFacingRight = true;
    private Animator _animator;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_player != null)
        {
            float currentDist = Vector2.Distance(transform.position, _player.transform.position);
            if (currentDist > distance)
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
                _animator.SetFloat("Speed", 0.06f);
            }
            else
            {
                _animator.SetFloat("Speed", 0.04f);
            }

            if (_player.transform.position.x > this.transform.position.x && !_isFacingRight)
            {
                Flip();
            }else if (_player.transform.position.x < this.transform.position.x && _isFacingRight)
            {
                Flip();
            }

        }
    }
    public void Flip()
    {
        _isFacingRight = !_isFacingRight;
        this.transform.Rotate(0, 180, 0);
    }
}
