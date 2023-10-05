using System.Collections;
using Player.Factory;
using Player.States;
using UnityEngine;

namespace Player
{
    /**
 * Developer note: This is temporary script to check the POC
 * Please for the love of god, make this into a state-machine
 * The entire code is begging to become a state-machine.
 */

    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PlayerContext : MonoBehaviour
    {
        public const float HorizontalDecelerationThreshold = 0.01f;
        public const float GravityFallMultiplier = 2.0f;
        public const float JumpBufferTime = 0.1f;
        public const float CoyoteTime = 0.2f;

        public PlayerBaseState PlayerState;
        public PlayerInput PlayerInput;
        public PlayerApplicable PlayerApplicable;

        public LayerMask EnvironmentLayer => environmentLayer;
        public float MaxHorizontalVelocity => maxHorizontalVelocity;
        public float HorizontalAcceleration => horizontalAcceleration;
        public float HorizontalDeceleration => horizontalDeceleration;
        public float UpwardForce => upwardForce;
        public float MaxDownwardVelocity => maxDownwardVelocity;
        public float DashPower => dashPower;
        public float DashTime => dashTime;
        public float BaseGravityScale => _baseGravityScale;
        
    
        public new Rigidbody2D rigidbody2D;

        [SerializeField] private LayerMask environmentLayer;
        [SerializeField] private float maxHorizontalVelocity = 5.0f;
        [SerializeField] private float horizontalAcceleration = 8.0f;
        [SerializeField] private float horizontalDeceleration = 10.0f;
        [SerializeField] private float upwardForce = 20.0f;
        [SerializeField] private float maxDownwardVelocity = 25.0f;
        [SerializeField] private float dashPower = 15.0f;
        [SerializeField] private float dashTime = 0.15f;
    
        [SerializeField] private ParticleSystem dashParticles;
    
        public Vector2 velocity;
        public float lastHorizontalInput;
        private float _baseGravityScale;
        
        public float coyoteTimer;


        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();

            PlayerStateFactory stateFactory = new PlayerStateFactory(this);
        
            PlayerState = stateFactory.Fall();
            PlayerState.EnterState();
        }

        private void Start()
        {
            _baseGravityScale = rigidbody2D.gravityScale;
            dashParticles.Stop();
        }

        private void Update()
        {
            PlayerState.UpdateStates();
            PlayerInput.UpdateInputs();

            /*
            if (
                PlayerInput.PressedSpace && 
                _canDash && 
                coyoteTimer <= 0.0f && 
                Mathf.Abs(rigidbody2D.velocity.x) > 0.0f && 
                !Grounded()
            ) {
                StartCoroutine(Dash());
            }
            */
        }
        
        public void SetGravityScale(float newGravityScale)
        {
            rigidbody2D.gravityScale = newGravityScale;
        }

        public void SetVelocity(float x, float y)
        {
            rigidbody2D.velocity = new Vector2(x, y);
        }

        public bool Grounded()
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
            Vector2 direction = PlayerInput.InputDirection.normalized;
            dashParticles.Play();
        
            SetGravityScale(0.0f);
            rigidbody2D.velocity = direction * dashPower;

            yield return new WaitForSeconds(dashTime);

            rigidbody2D.gravityScale = _baseGravityScale;
            dashParticles.Stop();
        }
    }
}
