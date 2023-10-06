using Player.Enums;
using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

        private Vector2 _lastDirection;
        private float _fallGravityScale;

        protected override void OnEnter()
        {
        }

        protected override void OnLeave()
        {
            Context.SetGravityScale();
        }

        protected override void OnUpdate()
        {
            Context.SetVelocity(
                Context.velocity.x * _lastDirection.x, 
                Mathf.Max(Context.rigid.velocity.y, PlayerStateMachine.MaxDownwardVelocity)
            );
            
            Context.Movement.Update(Context);
        }

        protected override void CanUpdateState()
        {
            if (Context.grounded && Mathf.Abs(Context.rigid.velocity.x) > 0.0f)
            {
                SwitchState(Factory.Move());
            }

            if (Context.grounded && Context.rigid.velocity.x == 0.0f) 
            {
                SwitchState(Factory.Idle());
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
