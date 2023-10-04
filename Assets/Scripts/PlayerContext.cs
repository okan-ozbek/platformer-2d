using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/**
 * Developer note: This is temporary script to check the POC
 * Please for the love of god, make this into a state-machine
 * The entire code is begging to become a state-machine.
 */

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerContext : MonoBehaviour
{
    private const float HorizontalDecelerationThreshold = 0.01f;
    private const float GravityFallMultiplier = 2.0f;
    
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float maxHorizontalVelocity = 5.0f;
    [SerializeField] private float horizontalAcceleration = 8.0f;
    [SerializeField] private float horizontalDeceleration = 10.0f;
    [SerializeField] private float upwardForce = 20.0f;
    [SerializeField] private float maxDownwardVelocity = 25.0f;
    [SerializeField] private float dashingPower = 15.0f;
    [SerializeField] private float dashTime = 0.15f;
    
    [SerializeField] private ParticleSystem dashParticles;

    private Vector2 _inputDirection;
    private bool _pressedJump;
    private bool _releasedJump;

    private Vector2 _velocity;
    private float _lastHorizontalInput;
    private float _baseGravityScale;

    private bool _canDash = true;
    private bool _isDashing = false;
    
    private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    private float _jumpBufferTime = 0.1f;
    private float _jumpBufferCounter;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _baseGravityScale = _rigidbody2D.gravityScale;
        dashParticles.Stop();
    }

    private void Update()
    {
        if (_isDashing)
        {
            return;
        }
        
        _inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _pressedJump = Input.GetButtonDown("Jump");
        _releasedJump = Input.GetButtonUp("Jump");

        HorizontalMovement();
        VerticalMovement();
        
        if (_pressedJump)
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (
            _pressedJump && 
            _canDash && 
            _coyoteTimeCounter <= 0.0f && 
            Mathf.Abs(_rigidbody2D.velocity.x) > 0.0f && 
            !Grounded()
        ) {
            StartCoroutine(Dash());
        }
        
        if (Grounded())
        {
            _coyoteTimeCounter = _coyoteTime;
            _canDash = true;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            _coyoteTimeCounter = Mathf.Max(_coyoteTimeCounter, 0.0f);
        }
    }
    private void HorizontalMovement()
    {
        WhenMoving();
        WhenNotMoving();
    }

    private void VerticalMovement()
    {
        Jump();
        Fall();
    }

    private void WhenMoving()
    {
        if (_inputDirection.x != 0.0f)
        {
            _velocity.x += horizontalAcceleration * Time.fixedDeltaTime;
            _velocity.x = Mathf.Min(_velocity.x, maxHorizontalVelocity);
            
            if (Mathf.RoundToInt(_lastHorizontalInput) != Mathf.RoundToInt(_inputDirection.x))
            {
                _lastHorizontalInput = _inputDirection.x;
                _velocity.x *= 0.3f;
            }
            
            _rigidbody2D.velocity = new Vector2(_velocity.x * _inputDirection.x, _rigidbody2D.velocity.y);
        }
    }

    private void WhenNotMoving()
    {
        if (_velocity.x > 0.0f && _inputDirection.x == 0.0f)
        {
            _velocity.x -= horizontalDeceleration * Time.fixedDeltaTime;

            if (_velocity.x <= HorizontalDecelerationThreshold)
            {
                _velocity.x = 0.0f;
            }
            
            _rigidbody2D.velocity = new Vector2(_velocity.x * _lastHorizontalInput, _rigidbody2D.velocity.y);
        }
    }

    private void Jump()
    {
        if (_coyoteTimeCounter > 0.0f && _jumpBufferCounter > 0.0f)
        {
            SetGravityScale(_baseGravityScale);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, upwardForce);

            _jumpBufferCounter = 0.0f;
        }

        if (_releasedJump && _rigidbody2D.velocity.y > 0.0f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.5f);

            _coyoteTimeCounter = 0.0f;
        }
    }

    private void Fall()
    {
        if (_rigidbody2D.velocity.y < 0.0f)
        {
            SetGravityScale(_baseGravityScale * GravityFallMultiplier);

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Mathf.Max(_rigidbody2D.velocity.y, -maxDownwardVelocity));
        }

        if (_rigidbody2D.velocity.y == 0.0f)
        {
            SetGravityScale(_baseGravityScale);
        }
    }

    private void SetGravityScale(float newGravityScale)
    {
        _rigidbody2D.gravityScale = newGravityScale;
    }

    private bool Grounded()
    {
        const float radius = 0.125f;
        
        Vector3 feet = transform.position;
        feet.y -= (float)(transform.localScale.y * 0.5);
        
        return Physics2D.OverlapCircle(feet, radius, environmentLayer);
    }

    private void OnDrawGizmos()
    {
        const float radius = 0.125f;
        
        Vector3 feet = transform.position;
        feet.y -= (float)(transform.localScale.y * 0.5);

        Gizmos.DrawSphere(feet, radius);
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        
        Vector2 direction = _inputDirection.normalized;

        dashParticles.Play();
        
        _rigidbody2D.gravityScale = 0.0f;
        _rigidbody2D.velocity = direction * dashingPower;

        yield return new WaitForSeconds(dashTime);

        _rigidbody2D.gravityScale = _baseGravityScale;
        _isDashing = false;
        dashParticles.Stop();
    }
}
