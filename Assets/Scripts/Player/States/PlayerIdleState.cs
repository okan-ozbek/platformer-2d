using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }

        protected override void OnEnter()
        {
            
        }

        protected override void OnLeave()
        {
            
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void CanUpdateState()
        {
            if (Context.Input.JumpBufferAvailable && Context.CoyoteTimeAvailable)
            {
                SwitchState(Factory.Jump()); 
            }

            if (Context.rigid.velocity.y < 0.0f)
            {
                SwitchState(Factory.Fall());
            }

            if (Context.Input.Direction.x != 0.0f)
            {
                SwitchState(Factory.Move());
            }
        }
    }
}
