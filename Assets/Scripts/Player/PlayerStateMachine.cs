using Player.Factories;
using Player.States;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
    public sealed class PlayerStateMachine : MonoBehaviour
    {
        public bool CoyoteTimeAvailable { get; private set; }
        public bool WallJumpAvailable { get; private set; }

        public const float FallThreshold = 0.15f;
        private const float DefaultCoyoteTime = 0.1f;
        private const float DefaultWallJumpTime = 0.1f;

        public Transform groundCheck;
        public Transform wallCheck;
        public LayerMask environmentLayer;
        public LayerMask deathLayer;
        public GameObject dashGameObject;

        public float horizontalDeceleration = 16.0f;
        public float horizontalAcceleration = 9.0f;
        public float maxHorizontalVelocity = 4.0f;
        public float maxDownwardVelocity = -20.0f;
        public float dashPower = 10.0f;
        public float upwardForce = 10.0f;
        public float wallSlideVelocity = -1.5f;
        public Vector2 wallJumpingPower = new(8, 16);
        public Vector3 spawnPosition;
        
        public bool DEBUG_RESET_STATE = false;

        [HideInInspector] public PlayerBaseState State;
        [HideInInspector] public PlayerInput Input;
        [HideInInspector] public PlayerMovement Movement;
        [HideInInspector] public PlayerAnimation Animation;
        [HideInInspector] public PlayerStateFactory Factory;
        [HideInInspector] public Rigidbody2D rigid;
        [HideInInspector] public Animator anim;
        [HideInInspector] public SpriteRenderer sprite;
        
        [HideInInspector] public bool canDash;
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool wallCollision;
        [HideInInspector] public bool deathCollision;
        [HideInInspector] public float defaultGravityScale;
        [HideInInspector] public Vector2 velocity;
        [HideInInspector] public Vector3 scale;
        [HideInInspector] public ParticleSystem dashParticleSystem;
        [HideInInspector] public PlayerDeathManager deathManager;

        private float _coyoteBufferTimer;
        private float _wallJumpBufferTimer;
        
        private void Start()
        {
            Input = new PlayerInput();
            Movement = new PlayerMovement();
            Animation = new PlayerAnimation();
            Factory = new PlayerStateFactory(this);
            
            rigid = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            dashParticleSystem = dashGameObject.GetComponent<ParticleSystem>();
            deathManager = GetComponent<PlayerDeathManager>();
            
            defaultGravityScale = rigid.gravityScale;

            State = Factory.Fall();
            State.Initialize();

            scale = transform.localScale;

            dashParticleSystem.Stop();
            spawnPosition = transform.position;
        }

        private void Update()
        {
            State.Update();
            Input.Update();
            
            SetCoyoteTime();
            SetWallJumpTime();
            ResetDash();
            
            Debug.Log(State.GetType());
            
            if (DEBUG_RESET_STATE)
            {
                State = Factory.Idle();
                DEBUG_RESET_STATE = false;
            }
        }

        private void FixedUpdate()
        {
            grounded = GetGrounded();
            wallCollision = GetWallCollision();
            deathCollision = GetDeathCollision();
        }

        private bool GetGrounded()
        {
            const float collisionDetectionRadius = 0.125f;

            return Physics2D.OverlapCircle(groundCheck.position, collisionDetectionRadius, environmentLayer);
        }
        
        private bool GetDeathCollision()
        {
            const float collisionDetectionRadius = 0.125f;

            return (
                Physics2D.OverlapCircle(groundCheck.position, collisionDetectionRadius, deathLayer) ||
                    transform.position.y < -30.0f
                );
        }

        private bool GetWallCollision()
        {
            const float collisionDetectionRadius = 0.1f;

            return Physics2D.OverlapCircle(wallCheck.position, collisionDetectionRadius, environmentLayer);
        }

        private void SetCoyoteTime()
        {
            _coyoteBufferTimer = (grounded) 
                ? DefaultCoyoteTime 
                : Mathf.Max(0.0f, _coyoteBufferTimer - Time.deltaTime);

            CoyoteTimeAvailable = (_coyoteBufferTimer > 0.0f);
        }

        private void SetWallJumpTime()
        {
            _wallJumpBufferTimer = (wallCollision)
                ? DefaultWallJumpTime
                : Mathf.Max(0.0f, _wallJumpBufferTimer - Time.deltaTime);

            WallJumpAvailable = (_wallJumpBufferTimer > 0.0f);
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

        public void DisableWallJumpTime()
        {
            _wallJumpBufferTimer = 0.0f;
            WallJumpAvailable = false;
        }
        
        private void ResetDash()
        {
            if (grounded)
            {
                canDash = true;
            }
        }
        
        private void OnDrawGizmos()
        {
            const float collisionDetectionRadius = 0.125f;

            Gizmos.DrawCube(wallCheck.position, Vector3.one * collisionDetectionRadius);
            Gizmos.DrawCube(groundCheck.position, Vector3.one * collisionDetectionRadius);
        }
    }
}
