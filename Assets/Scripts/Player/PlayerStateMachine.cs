using Player.Factories;
using Player.States;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public sealed class PlayerStateMachine : MonoBehaviour
    {
        public bool CoyoteTimeAvailable { get; private set; }

        private const float DefaultCoyoteTime = 0.1f;
        
        public float horizontalDeceleration = 16.0f;
        public float horizontalAcceleration = 9.0f;
        public float maxHorizontalVelocity = 4.0f;
        public float maxDownwardVelocity = -20.0f;
        public float dashPower = 10.0f;
        public float upwardForce = 10.0f;
        
        [HideInInspector] public PlayerBaseState State;
        [HideInInspector] public PlayerInput Input;
        [HideInInspector] public PlayerMovement Movement;
        [HideInInspector] public Rigidbody2D rigid;

        [HideInInspector] public bool canDash;
        [HideInInspector] public bool grounded;
        [HideInInspector] public float defaultGravityScale;
        [HideInInspector] public LayerMask environmentLayer;
        [HideInInspector] public Vector2 velocity;
        
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
