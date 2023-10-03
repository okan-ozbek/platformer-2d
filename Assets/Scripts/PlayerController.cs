using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    private float _horizontalInputDirection;
    private bool _hasJumped;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _horizontalInputDirection = Input.GetAxisRaw("Horizontal");
        _hasJumped = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rigidbody2D.AddForce(new Vector2(_horizontalInputDirection * speed, 0.0f), ForceMode2D.Force);

        if (_horizontalInputDirection == 0.0f)
        {
            _rigidbody2D.drag = 10.0f;
        }

        var clampedVelocityX = _rigidbody2D.velocity.x;
        if (Mathf.Abs(_rigidbody2D.velocity.x) > 0.0f)
        {
            clampedVelocityX = Mathf.Sign(_rigidbody2D.velocity.x) * maxSpeed;
        }

        _rigidbody2D.velocity = new Vector2(clampedVelocityX, _rigidbody2D.velocity.y);

    }
}
