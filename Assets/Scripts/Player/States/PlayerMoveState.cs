using System;
using Player.Enums;
using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory) { }
        
        protected override void OnEnter()
        {
            Context.Animation.ChangeAnimation(Context, PlayerAnimationState.Move);
        }

        protected override void OnLeave()
        {
            
        }

        protected override void OnUpdate()
        {
            Context.Movement.Update(Context);
        }

        protected override void CanUpdateState()
        {
            if (Context.Input.JumpBufferAvailable && Context.CoyoteTimeAvailable)
            {
                SwitchState(Factory.Jump());
            }

            if (Context.rigid.velocity.y < (0.0f - PlayerStateMachine.FallThreshold))
            {
                SwitchState(Factory.Fall());
            }
            
            if (Context.Input.Direction.x == 0.0f && Context.velocity.x == 0.0f)
            {
                SwitchState(Factory.Idle());
            }
        }
    }
}
