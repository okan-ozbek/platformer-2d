using Player.Factory;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }
        
        public override void EnterState()
        {
            Context.coyoteTimer = PlayerContext.CoyoteTime;
            
            Debug.Log("hit");
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        { }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.JumpState)
            {
                SwitchState(StateFactory.Jump());
            }

            if (Context.PlayerApplicable.FallState)
            {
                SwitchState(StateFactory.Fall());
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