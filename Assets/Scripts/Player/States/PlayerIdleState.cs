using Player.Factory;
using UnityEngine;

namespace Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory) { }
        
        public override void EnterState()
        { }

        public override void UpdateState()
        {
            CheckSwitchStates();
            
            if (Context.velocity.x > 0.0f)
            {
                Context.velocity.x -= Context.HorizontalDeceleration * Time.fixedDeltaTime;

                if (Context.velocity.x <= PlayerContext.HorizontalDecelerationThreshold)
                {
                    Context.velocity.x = 0.0f;
                }
            
                Context.SetVelocity(Context.velocity.x * Context.lastHorizontalInput, Context.rigidbody2D.velocity.y);
            }
        }

        public override void ExitState()
        { }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.MoveState)
            {
                SwitchState(StateFactory.Move());
            }
        }

        public override void InitializeSubState()
        { }
    }
}