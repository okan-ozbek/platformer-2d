using System;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(BoxCollider2D)
)]
public class PlayerMovement : MonoBehaviour
{
    private bool DidDirectionChange => (DirectionChangeLeft() || DirectionChangeRight());
    private bool CanJump => (_jumped && OnGround());
    
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private LayerMask environmentLayer;

    [SerializeField] private float movementAcceleration;
    [SerializeField] private float terminalMovementSpeed;
    [SerializeField] private float linearDrag;

    private float _horizontalMovementInput;

    [SerializeField] private float upwardForce;

    private bool _jumped;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMovementInput = MovementInput().x;

        Jump();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    
    private Vector2 MovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Movement()
    {
        _rigidbody2D.AddForce(Vector2.right * _horizontalMovementInput * movementAcceleration);
        
        SetTerminalVelocityX();
        SetRigidbodyDrag();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnGround())
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, upwardForce);
        }

        if (Input.GetKeyUp(KeyCode.Space) && _rigidbody2D.velocity.y > 0.0f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.5f);
        }
    }

    private bool OnGround()
    {
        Vector3 feet = new Vector3(
            transform.position.x, 
            (float)(transform.position.y - (transform.localScale.y * 0.5)),
            transform.position.z
        );

        Debug.Log(Physics2D.OverlapCircle(feet, 0.25f, environmentLayer));
        
        return Physics2D.OverlapCircle(feet, 0.25f, environmentLayer);
    }
    
    private void SetTerminalVelocityX()
    {
        float terminalVelocityX = Mathf.Clamp(_rigidbody2D.velocity.x, -terminalMovementSpeed, terminalMovementSpeed);
        
        _rigidbody2D.velocity = new Vector2(terminalVelocityX, _rigidbody2D.velocity.y);
    }
    
    private void SetRigidbodyDrag()
    {
        _rigidbody2D.drag = (HasMovementStopped() || DidDirectionChange)
            ? linearDrag
            : 0.0f;
    }
    
    private bool DirectionChangeLeft()
    {
        return (_rigidbody2D.velocity.x > 0.0f && _horizontalMovementInput < 0.0f);
    }

    private bool DirectionChangeRight()
    {
        return (_rigidbody2D.velocity.x < 0.0f && _horizontalMovementInput > 0.0f);
    }

    private bool HasMovementStopped()
    {
        return (_horizontalMovementInput == 0.0f);
    }

    private void OnDrawGizmos()
    {
        Vector3 feet = new Vector3(
            transform.position.x, 
            (float)(transform.position.y - (transform.localScale.y * 0.5)),
            transform.position.z
        );
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(feet, Vector3.down * 0.25f);
    }
}