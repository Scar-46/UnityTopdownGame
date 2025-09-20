using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

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
    private AfterImagePool _afterImagePool;

    private GameObject camera;
    public GameObject minimapIcon;
    public GameObject weapon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _afterImagePool = GetComponent<AfterImagePool>();
        _canDash = true;
        minimapIcon.SetActive(false);
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
            Vector2 dashDirection;
            if (_playerInput != Vector2.zero)
                dashDirection = _playerInput;
            else
                dashDirection = _isFacingRight ? Vector2.right : Vector2.left;

            StartCoroutine(Dash(dashDirection));
        }
    }

    private void SetAnimation(Vector2 direction)
    {
        _animator.SetFloat("Speed", direction.sqrMagnitude);
    }

    private IEnumerator Dash(Vector2 dashDirection)
    {
        _canDash = false;
        _isDashing = true;
        _animator.SetTrigger("Dash");
        AudioManager.Instance.Play("Dash");

        _rigidbody.velocity = dashDirection.normalized * dashSpeed;
        float elapsed = 0f;
        float dashAfterImageTimer = 0f;

        while (elapsed < _dashDuration)
        {
            // Move player
            _rigidbody.velocity = dashDirection.normalized * dashSpeed;

            // Spawn after-image

            dashAfterImageTimer += Time.deltaTime;
            if (dashAfterImageTimer >= _afterImagePool.spawnRate)
            {
                _afterImagePool.SpawnAfterImage();
                dashAfterImageTimer = 0f;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
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