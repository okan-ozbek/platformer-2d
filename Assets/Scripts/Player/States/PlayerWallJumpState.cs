using Player.Enums;
using Player.Factories;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerWallJumpState : PlayerBaseState
    {
        public PlayerWallJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        private const float NoMovementTime = 0.1f;
        
        private float _noMovementTimer;
        private Vector2 _wallJumpingPower;

        protected override void OnEnter()
        {
            Context.Animation.ChangeAnimation(Context, PlayerAnimationState.Jump);
            
            _wallJumpingPower = Context.wallJumpingPower;
            Context.velocity = _wallJumpingPower;

            float horizontalForceDirection = (Context.wallCollision)
                ? -Mathf.Sign(Context.transform.localScale.x)
                : Mathf.Sign(Context.transform.localScale.x);
            
            Context.rigid.velocity = new Vector2(Context.velocity.x * horizontalForceDirection, Context.velocity.y);

            Context.DisableWallJumpTime();
            _noMovementTimer = NoMovementTime;
        }

        protected override void OnLeave()
        {
        }

        protected override void OnUpdate()
        {
            if (_noMovementTimer <= 0.0f)
            {
                Context.Movement.Update(Context);    
            }

            _noMovementTimer -= Time.deltaTime;
        }

        protected override void CanUpdateState()
        {
            if (Context.rigid.velocity.y < 0.0f)
            {
                SwitchState(Factory.Fall());
            }

            if (
                (Context.Input.JumpBufferAvailable || Context.Input.PressedSpace) &&
                Context.Input.Direction.x != 0.0f &&
                Context.canDash
            )
            {
                SwitchState(Factory.Dash());
            }
        }
    }
}