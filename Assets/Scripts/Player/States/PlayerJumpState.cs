using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

        private const float UpwardForce = 10.0f;

        private Vector2 _lastDirection;
        
        protected override void OnEnter()
        {
            Context.SetGravityScale();
            
            Context.SetVelocity(Context.velocity.x * _lastDirection.x, UpwardForce);

            Context.Input.DisableJumpBuffer();
            Context.DisableCoyoteTime();
        }

        protected override void OnLeave()
        {
            
        }

        protected override void OnUpdate()
        {
            if (Context.Input.ReleasedSpace)
            {
                Context.SetVelocity(Context.velocity.x * _lastDirection.x, Context.rigid.velocity.y * 0.5f);
            }
            
            Context.Movement.Update(Context);
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
            ) {
                SwitchState(Factory.Dash());
            }
        }
    }
}
