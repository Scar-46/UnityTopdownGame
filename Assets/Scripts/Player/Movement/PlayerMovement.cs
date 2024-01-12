using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    private Rigidbody2D _rigidbody;
    private Vector2 _playerInput;
    private Animator _animator;
    private bool _isFacingRight = true;

    [Header("Dash settings")]
    public float dashSpeed;
    private float _dashCooldown = 1f;
    private float _dashDuration = 0.5f;
    private bool _isDashing;
    private bool _canDash;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _canDash = true;
    }

    private void Update()
    {
        if (!_isDashing)
        {
            ReadInput();
            SetAnimation(_playerInput);

        }
    }

    private void ReadInput()
    {
        _playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _rigidbody.velocity = _playerInput * speed;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;


        if (!_isFacingRight && (mousePos.x > playerPos.x))
        {
            Flip();
        }
        else if (_isFacingRight && (mousePos.x < playerPos.x))
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _canDash)
        {
            StartCoroutine(Dash());
        }


    }

    private void SetAnimation(Vector2 direction)
    {
        _animator.SetFloat("AxisX", direction.x);
        _animator.SetFloat("AxisY", direction.y);
        _animator.SetFloat("Speed", direction.sqrMagnitude);

    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        _rigidbody.velocity = _playerInput * dashSpeed;
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    public void Flip()
    {
        _isFacingRight = !_isFacingRight;
        this.transform.Rotate(0, 180, 0);
    }
}