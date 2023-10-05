using Player.Factory;
using UnityEngine;

namespace Player.States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory) { }
        
        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            
            Context.velocity.x += Context.HorizontalAcceleration * Time.fixedDeltaTime;
            Context.velocity.x = Mathf.Min(Context.velocity.x, Context.MaxHorizontalVelocity);
            
            if (Mathf.RoundToInt(Context.lastHorizontalInput) != Mathf.RoundToInt(Context.PlayerInput.InputDirection.x))
            {
                Context.lastHorizontalInput = Context.PlayerInput.InputDirection.x;
                Context.velocity.x *= 0.3f;
            }
            
            Context.SetVelocity(Context.velocity.x * Context.PlayerInput.InputDirection.x, Context.rigidbody2D.velocity.y);
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.IdleState)
            {
                SwitchState(StateFactory.Idle());
            }
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }
    }
}