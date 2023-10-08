using Player.Enums;
using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

        private const float NoMovementTime = 0.1f;
        
        private float _noMovementTimer;
        
        protected override void OnEnter()
        {
            Context.Animation.ChangeAnimation(Context, PlayerAnimationState.Jump);
            Context.SetGravityScale();
            
            Context.SetVelocity(Context.velocity.x * Context.Input.LastDirection.x, Context.upwardForce);

            Context.Input.DisableJumpBuffer();
            Context.DisableCoyoteTime();
            
            _noMovementTimer = NoMovementTime;
        }

        protected override void OnLeave()
        {
        }

        protected override void OnUpdate()
        {
            if (Context.Input.ReleasedSpace)
            {
                Context.SetVelocity(Context.velocity.x * Context.Input.LastDirection.x, Context.rigid.velocity.y * 0.5f);
            }
            
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
