﻿using Player.Enums;
using Player.Factories;
using UnityEngine;

namespace Player.States
{
    public sealed class PlayerWallSlideState : PlayerBaseState
    {
        public PlayerWallSlideState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        protected override void OnEnter()
        {
            Context.Animation.ChangeAnimation(Context, PlayerAnimationState.Slide);
        }

        protected override void OnLeave()
        {
        }

        protected override void OnUpdate()
        {
            Context.Movement.Update(Context);
            
            Context.SetVelocity(Context.rigid.velocity.x, Context.wallSlideVelocity);
        }

        protected override void CanUpdateState()
        {
            if (Context.grounded)
            {
                SwitchState(
                    (Context.Input.Direction.x == 0.0f)
                        ? Factory.Idle()
                        : Factory.Move()
                );
            }

            if (
                !Context.grounded &&
                Context.rigid.velocity.y < 0.0f &&
                !Context.WallJumpAvailable
            ) {
                SwitchState(Factory.Fall());
            }

            if (
                !Context.grounded &&
                Context.Input.PressedSpace &&
                Context.WallJumpAvailable
            ) { 
                SwitchState(Factory.WallJump());
            }
        }
    }
}