using Player.Factories;
using Player.States;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public sealed class PlayerStateMachine : MonoBehaviour
    {
        public bool CoyoteTimeAvailable { get; private set; }

        private const float DefaultCoyoteTime = 0.1f;
        public const float HorizontalDeceleration = 8.0f;
        public const float HorizontalAcceleration = 10.0f;
        public const float MaxHorizontalVelocity = 5.0f;
        public const float MaxDownwardVelocity = -20.0f;
        
        public PlayerBaseState State;
        public PlayerInput Input;
        public PlayerMovement Movement;
        public Rigidbody2D rigid;

        public bool canDash;
        public bool grounded;
        public float defaultGravityScale;
        public LayerMask environmentLayer;
        public Vector2 velocity;
        
        private float _coyoteBufferTimer;
        
        private void Start()
        {
            Input = new PlayerInput();
            Movement = new PlayerMovement();
            
            PlayerStateFactory factory = new PlayerStateFactory(this);
            rigid = GetComponent<Rigidbody2D>();

            defaultGravityScale = rigid.gravityScale;

            State = factory.Fall();
            State.Initialize();
        }

        private void Update()
        {
            State.Update();
            Input.Update();
            
            SetCoyoteTime();
            ResetDash();
        }

        private void FixedUpdate()
        {
            grounded = GetGrounded();
        }

        private bool GetGrounded()
        {
            const float collisionDetectionRadius = 0.125f;
            
            Vector3 feetPosition = transform.position;
            feetPosition.y -= transform.localScale.y * 0.5f;

            return Physics2D.OverlapCircle(feetPosition, collisionDetectionRadius, environmentLayer);
        }

        private void SetCoyoteTime()
        {
            _coyoteBufferTimer = (grounded) 
                ? DefaultCoyoteTime 
                : Mathf.Max(0.0f, _coyoteBufferTimer - Time.deltaTime);

            CoyoteTimeAvailable = (_coyoteBufferTimer > 0.0f);
        }

        public void SetVelocity(float x, float y)
        {
            rigid.velocity = new Vector2(x, y);
        }
        
        public void SetVelocity(Vector2 v)
        {
            rigid.velocity = new Vector2(v.x, v.y);
        }

        public void SetGravityScale(float? gravityScale = null)
        {
            rigid.gravityScale = gravityScale ?? defaultGravityScale;
        }

        public void DisableCoyoteTime()
        {
            _coyoteBufferTimer = 0.0f;
            CoyoteTimeAvailable = false;
        }
        
        private void ResetDash()
        {
            if (grounded)
            {
                canDash = true;
            }
        }
    }
}
