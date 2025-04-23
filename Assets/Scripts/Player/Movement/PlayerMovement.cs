using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement settings")]
    public float speed;
    private Rigidbody2D _rigidbody;
    private Vector2 _playerInput;
    private Animator _animator;
    private bool _isFacingRight = true;

    [Header("Dash settings")]
    public float dashSpeed;
    public float _dashCooldown = 1f;
    private float _dashDuration = 0.4f;
    private bool _isDashing;
    private bool _canDash;

    private GameObject camera;
    public GameObject minimapIcon;
    public GameObject weapon;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        minimapIcon.SetActive(true);
        _canDash = true;
        camera = GameObject.FindGameObjectWithTag("Camera");
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
        _animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        _animator.SetTrigger("Dash");
        AudioManager.Instance.Play("Dash");
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

    public void DisableMovement(bool activate)
    {
        foreach (var comp in weapon.GetComponents<PlayerAttack>())
        {
            comp.attackBlocked = activate;
        }
        camera.SetActive(activate);
    }
}