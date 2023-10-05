﻿using UnityEngine;
using Player.Factory;

namespace Player.States
{
    public sealed class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerContext context, PlayerStateFactory stateFactory) : base(context, stateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }
        
        public override void EnterState()
        {
            Context.SetGravityScale(Context.BaseGravityScale * PlayerContext.GravityFallMultiplier);
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            
            Context.coyoteTimer -= Time.deltaTime;
            Context.coyoteTimer = Mathf.Max(Context.coyoteTimer, 0.0f);
            
            Context.SetVelocity(Context.rigidbody2D.velocity.x, Mathf.Max(Context.rigidbody2D.velocity.y, -Context.MaxDownwardVelocity));
        }

        public override void ExitState()
        {
            Context.SetGravityScale(Context.BaseGravityScale);
        }

        public override void CheckSwitchStates()
        {
            if (Context.PlayerApplicable.DashState)
            {
                SwitchState(StateFactory.Dash());
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