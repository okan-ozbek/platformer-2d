using Player.Factory;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }
        
        public override void EnterState()
        {
            Context.SetGravityScale(Context.BaseGravityScale);
            
            Context.SetVelocity(Context.rigidbody2D.velocity.x, Context.UpwardForce);
            
            Context.PlayerInput.JumpBufferTimer = 0.0f;
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            
            if (Context.PlayerInput.ReleasedSpace && Context.rigidbody2D.velocity.y > 0.0f)
            {
                Context.SetVelocity(Context.rigidbody2D.velocity.x, Context.rigidbody2D.velocity.y * 0.5f);

                Context.PlayerInput.JumpBufferTimer = 0.0f;
            }
        }

        public override void ExitState()
        { }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.GroundedState)
            {
                SwitchState(StateFactory.Grounded());
            }
            
        }

        public override void InitializeSubState()
        {
            if (Context.PlayerApplicable.MoveState)
            {
                SetSubState(StateFactory.Move());
            }

            if (Context.PlayerApplicable.IdleState)
            {
                SetSubState(StateFactory.Idle());
            }
        }
    }
}